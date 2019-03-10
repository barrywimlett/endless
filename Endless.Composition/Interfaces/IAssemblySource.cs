using System.Collections.Generic;
using System.Reflection;

namespace Endless.Composition.Interfaces
{
    public interface IAssemblySource
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}