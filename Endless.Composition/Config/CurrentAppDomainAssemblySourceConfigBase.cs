using System.Runtime.Serialization;
using Endless.Composition.Interfaces;
using Endless.Composition.Sources;

namespace Endless.Composition.Config
{
    //[Serializable]
    [DataContract(Namespace = "Endless.Composition.Config")]
    public class CurrentAppDomainAssemblySourceConfig : AssemblySourceConfigBase {

    
        override public IAssemblySource GetFilterFromConfig(AssemblySourceConfigBase configBase)
        {
            return new CurrentAppDomainAssemblySource();
        }
    }
}