using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Endless.Composition.Config;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Filters
{
    public interface IFilterAssmblyHasAttributesConfig 
    {
        Type AttributeType { get; }

    }

    [Serializable]
    public class FilterAssemblyHasAttributesConfig : AssemblyFilterConfigBase, IFilterAssmblyHasAttributesConfig
    {
        public FilterAssemblyHasAttributesConfig()
        {

        }

        [XmlAttribute]
        public Type AttributeType { get; }
        override internal IAssemblyFilter GetFilterFromConfig(AssemblyFilterConfigBase config)
        {
            return new FilterAssemblyHasAttributes(config as IFilterAssmblyHasAttributesConfig);
        }

    }

    
    public class FilterAssemblyHasAttributes : IFilterAssmblyHasAttributesConfig,IAssemblyFilter
    {
        public FilterAssemblyHasAttributes(IFilterAssmblyHasAttributesConfig config)
        {
            AttributeType = config.AttributeType;
        }

        public FilterAssemblyHasAttributes(Type attributeType)
        {
            AttributeType = attributeType;
        }

        public Type AttributeType { get; protected set; }

        virtual public bool Filter(Assembly assembly)
        {
            bool hasAttribute = false;
            var attrCollection=assembly.GetCustomAttributes(AttributeType, false);

            hasAttribute = attrCollection.Any();
            
            return hasAttribute;
        }
    }
    
}