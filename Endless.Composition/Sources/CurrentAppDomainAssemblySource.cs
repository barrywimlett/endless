using System;
using System.Collections.Generic;
using System.Reflection;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Sources
{
    public class CurrentAppDomainAssemblySource : IAssemblySource
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}