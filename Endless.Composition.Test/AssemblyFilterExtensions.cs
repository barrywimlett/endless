using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Test
{
    static public class AssemblyFilterExtensions {

        static public IEnumerable<Assembly> FilteredBy(this IEnumerable<Assembly> assemblies, IAssemblyFilter filter)
        {
            return assemblies.Where(filter.Filter);
        }
    }
}