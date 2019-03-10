using System.Runtime.Serialization;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Config
{
    //[Serializable]
    //[XmlInclude(typeof(CurrentAppDomainAssemblySourceConfig))]
    //[XmlInclude(typeof(FolderAssemblySourceConfig))]
    [DataContract(Namespace = "Endless.Composition.Config")]
    abstract public class AssemblySourceConfigBase
    {
        
        abstract public IAssemblySource GetFilterFromConfig(AssemblySourceConfigBase config);
    }
}