using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Sources
{
    public class FolderAssemblySource : IAssemblySource
    {
        public FolderAssemblySource(IFolderAssemblySourceConfig config)
        {
            Path = config.Path;
        }

        public FolderAssemblySource(string path)
        {
            Path = path;
        }

        public string Path { get; protected set; }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return System.IO.Directory.EnumerateFiles(Path, "*.exe,*.dll")
                .Select(Assembly.ReflectionOnlyLoadFrom);
        }
    }
}