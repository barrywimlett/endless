using System.Reflection;

namespace Endless.Composition.Interfaces
{
    public interface IAssemblyFilter
    {
        bool Filter(Assembly assembly);
    }
}