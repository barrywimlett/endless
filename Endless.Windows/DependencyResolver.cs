// -----------------------------------------------------------------------
// <copyright file="DependencyResolver.cs" company="Solarvista">
//     Copyright (c) Solarvista. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Endless
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Composition.Hosting;
    using System.Diagnostics.Tracing; 
    using System.Reflection;
    using System.Xml.Linq;
    
    using System.Xml.Serialization;
    /// <summary>
    /// MEF 2 Dependency Resolver
    /// </summary>
    public sealed class DependencyResolver : Endless.IDependencyResolver
    {
        /// <summary>
        /// The dependency manifest file name
        /// </summary>
        public const string DependencyManifestFileName = "DependencyManifest.xml";

        /// <summary>
        /// The lazy instance
        /// </summary>
        private static readonly Lazy<DependencyResolver> LazyInstance = new Lazy<DependencyResolver>(() => new DependencyResolver());

        /// <summary>
        /// The host
        /// </summary>
        private CompositionHost host = null;

        /// <summary>
        /// Prevents a default instance of the <see cref="DependencyResolver"/> class from being created.
        /// </summary>
        private DependencyResolver()
        {
            this.host = this.Compost();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DependencyResolver Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public CompositionHost Container
        {
            get
            {
                return this.host;
            }
        }

        /// <summary>
        /// Exports the specified type.
        /// </summary>
        /// <param name="exportType">Type of the export.</param>
        /// <returns>The exported instance.</returns>
        public object Export(Type exportType)
        {
            return this.Container.GetExport(exportType);
        }

        /// <summary>
        /// Exports the specified type.
        /// </summary>
        /// <typeparam name="T">The type to export</typeparam>
        /// <returns>The exported instance.</returns>
        public T Export<T>()
        {
            return this.Container.GetExport<T>();
        }

        /// <summary>
        /// Gets a set of exports.
        /// </summary>
        /// <param name="exportType">Type of the export.</param>
        /// <returns>An enumeration of the exported type</returns>
        public IEnumerable<object> GetExports(Type exportType)
        {
            return this.Container.GetExports(exportType);
        }

        /// <summary>
        /// Gets a set of exports.
        /// </summary>
        /// <typeparam name="T">The export type</typeparam>
        /// <returns>An enumeration of the exported type</returns>
        public IEnumerable<T> GetExports<T>()
        {
            return this.Container.GetExports<T>();
        }

        /// <summary>
        /// Composts this instance.
        /// </summary>
        /// <returns>The <see cref="CompositionHost"/> instance.</returns>
        private CompositionHost Compost()
        {
            try
            {
                System.Composition.Hosting.ContainerConfiguration config = new ContainerConfiguration();
                
                LoadAssembliesFromManifest(config);
                
                var host = config.CreateContainer();

                return host;
            }
            catch (Exception ex)
            {
                //Logger.ErrorFormat("Error creating Mef host{0}Exception:{1}", Environment.NewLine, ex.CreateLogMessage());
                throw;
            }
        }


        public class Manifest
        {
            public Manifest()
            {
                Dependencies=new List<Assembly>();
            }
            public class Assembly
            {
                public Assembly()
                {
                    
                }
                public Assembly(string name)
                {
                    Name = name;
                }
                public string Name { get; set; }
            }

            public List<Assembly> Dependencies { get; set; }
        }
        public static void SaveAssembliesToManifest()
        {//ContainerConfiguration config
            Manifest manifest=new Manifest();
//            var binDir = AppDomain.CurrentDomain.BaseDirectory;
//            var manifestFilePath = Path.Combine(binDir, DependencyManifestFileName);

            var ass =AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in ass)
            {
                manifest.Dependencies.Add(new Manifest.Assembly(assembly.FullName));
            }

            using (var fileStream = File.Open("manifest2.xml", FileMode.Create, FileAccess.Write))
            {
                XmlSerializer s=new XmlSerializer(typeof(Manifest));
                s.Serialize(fileStream,manifest);
            }
        }
        public static void LoadAssembliesFromManifest(ContainerConfiguration config)
        {
            var binDir = AppDomain.CurrentDomain.BaseDirectory;
            var manifestFilePath = Path.Combine(binDir, DependencyManifestFileName);

            if (!File.Exists(manifestFilePath))
            {
                throw new InvalidOperationException("Could not find dependency manifest file at:" + manifestFilePath);
            }

            var doc = XDocument.Load(manifestFilePath);

            foreach (var assNode in doc.Root.Elements("Assembly"))
            {
                var assName = assNode.Value;
                
                try
                {
                    Trace.TraceInformation("loading Assembly {0} into composition host.", assName);

                    Assembly ass = Assembly.Load(assName);
                    
                    if (ass == null)
                    {
                        throw new InvalidOperationException("Failed to load dependency assembly file:" + assName);
                    }
                    
                    config.WithAssembly(ass);
                }
                catch
                {
                    Trace.TraceError("Error loading Assembly:{0}.", assName);
                    throw;
                }
            }
        }
    }
}