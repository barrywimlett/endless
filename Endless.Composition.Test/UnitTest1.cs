using System;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Endless.Composition.Config;
using Endless.Composition.Factories;
using Endless.Composition.Filters;
using Endless.Composition.Interfaces;
using Endless.Composition.Sources;

namespace Endless.Composition.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FilterAssemblyByName nameFilter=new FilterAssemblyByName(NameCheckTypeEnum.StartsWith,"Endless.");

            Config.Config cfg= new Config.Config();

            cfg.Sources.Add(new CurrentAppDomainAssemblySourceConfig());
            var filterAssemblyByNameConfig = new FilterAssemblyByNameConfig();
            filterAssemblyByNameConfig.ComparisonType = StringComparison.InvariantCultureIgnoreCase;
            filterAssemblyByNameConfig.NameCheckType = NameCheckTypeEnum.StartsWith;
            filterAssemblyByNameConfig.PartialName = "Endless.";

            cfg.Includes.Add(filterAssemblyByNameConfig);


            ContractResolver.singleton.RegisterContractsByReflection(typeof(ContractResolver).Assembly);

            using (var stream = System.IO.File.Open("config.xml", FileMode.Create, FileAccess.Write))
            {
                
                DataContractSerializerSettings settings= new DataContractSerializerSettings();
                settings.DataContractResolver = ContractResolver.singleton;

                DataContractSerializer ser =new DataContractSerializer(typeof(Config.Config),settings);
                //XmlSerializer ser = new XmlSerializer(typeof(Config.Config));
                //ser.Serialize(stream, cfg);

                ser.WriteObject(stream, cfg);

            }


            using (var stream = System.IO.File.Open("config.xml", FileMode.Open, FileAccess.Read))
            {

                DataContractSerializerSettings settings = new DataContractSerializerSettings();
                settings.DataContractResolver = ContractResolver.singleton;

                DataContractSerializer ser = new DataContractSerializer(typeof(Config.Config), settings);

                var cfg2 = ser.ReadObject(stream);
                Debug.Assert(false);
            }

            System.Composition.Hosting.ContainerConfiguration config = new ContainerConfiguration();

            var assemblies= new CurrentAppDomainAssemblySource().GetAssemblies();
            assemblies = assemblies.FilteredBy(nameFilter);

            config.WithAssemblies(assemblies);
            var host = config.CreateContainer();

            PurposeBasedFactory<ITestInterface> factory= new PurposeBasedFactory<ITestInterface>(host);

            var export=factory.GetExport();

            Debug.Assert(false);
        }
    }
}
