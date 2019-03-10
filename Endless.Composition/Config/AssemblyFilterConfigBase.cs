using System.Runtime.Serialization;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Config
{
    [DataContract(Namespace = "Endless.Composition.Config")]
    abstract public class AssemblyFilterConfigBase 
    {
        abstract internal IAssemblyFilter GetFilterFromConfig(AssemblyFilterConfigBase config);

    }
}