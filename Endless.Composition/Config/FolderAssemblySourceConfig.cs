using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Endless.Composition.Interfaces;
using Endless.Composition.Sources;

namespace Endless.Composition.Config
{

    //[Serializable]
    [DataContract(Name= "FolderAssemblySource" ,Namespace = "Endless.Composition.Config")]
    public class FolderAssemblySourceConfig : AssemblySourceConfigBase, IFolderAssemblySourceConfig {

        [DataMember]
        [XmlAttribute]
        public string Path { get; set; }

        override public IAssemblySource GetFilterFromConfig(AssemblySourceConfigBase configBase)
        {
            Contract.Requires(configBase != null);
            return GetFilterFromConfig(configBase as IFolderAssemblySourceConfig);
        }

        IAssemblySource GetFilterFromConfig(IFolderAssemblySourceConfig config)
        {
            Contract.Requires(config != null);

            return new FolderAssemblySource(config);
        }
    }
}