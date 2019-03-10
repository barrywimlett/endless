using System;
using System.Reflection;
using System.Xml.Serialization;
using Endless.Composition.Config;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Filters
{
    public interface IFilterAssemblyBySigningKeyConfig
    {
        byte[] PublicKeyToken
        {
            get;
        }

    }

    
    [Serializable]
    public class FilterAssemblyBySigningKeyConfig : AssemblyFilterConfigBase, IFilterAssemblyBySigningKeyConfig {
        public FilterAssemblyBySigningKeyConfig()
        {
            
        }
        [XmlAttribute]
        public byte[] PublicKeyToken
        {
            get;
            set;
        }

        override internal IAssemblyFilter GetFilterFromConfig(AssemblyFilterConfigBase config)
        {
            return new FilterAssemblyBySigningKey(config as IFilterAssemblyBySigningKeyConfig);
        }
        
    }

    public class FilterAssemblyBySigningKey : IAssemblyFilter, IFilterAssemblyBySigningKeyConfig
    {
        
        public FilterAssemblyBySigningKey(IFilterAssemblyBySigningKeyConfig config)
        {
            this.PublicKeyToken = config.PublicKeyToken;
        }

        public byte[] PublicKeyToken { 
            get;

            protected set;
        }

        public bool Filter(Assembly assembly)
        {
            return assembly.GetName().GetPublicKeyToken().Equals(PublicKeyToken);
        }
    }
}