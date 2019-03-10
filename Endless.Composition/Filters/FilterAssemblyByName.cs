using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Endless.Composition.Config;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Filters
{

    public enum NameCheckTypeEnum
    {
        StartsWith,
        EndsWith,
        Contains,
        Equals
    }

    public interface IFilterAssemblyByNameConfig
    {
        NameCheckTypeEnum NameCheckType { get; }

        string PartialName { get; }

        StringComparison ComparisonType { get; }

    }

    
    [DataContract(Name = "FilterAssemblyByName", Namespace = "Endless.Composition.Config")]
    public class FilterAssemblyByNameConfig : AssemblyFilterConfigBase, IFilterAssemblyByNameConfig {
        public FilterAssemblyByNameConfig()
        {

        }


        [DataMember]
        [XmlAttribute]
        public NameCheckTypeEnum NameCheckType { get; set; }
        [DataMember]
        [XmlAttribute]
        public string PartialName { get; set; }
        [DataMember]
        [XmlAttribute]
        public StringComparison ComparisonType { get; set; } 

        override internal IAssemblyFilter GetFilterFromConfig(AssemblyFilterConfigBase config)
        {
            return new FilterAssemblyByName(config as IFilterAssemblyByNameConfig);
        }

        
    }

    public class FilterAssemblyByName : IAssemblyFilter, IFilterAssemblyByNameConfig
    {
        
        public NameCheckTypeEnum NameCheckType { get; protected set; }
        public string PartialName { get; protected set; }

        public StringComparison ComparisonType { get; } = StringComparison.InvariantCultureIgnoreCase;

        public FilterAssemblyByName(IFilterAssemblyByNameConfig config)
        {
            NameCheckType = config.NameCheckType;
            PartialName = config.PartialName;
            ComparisonType = config.ComparisonType;
        }

        public FilterAssemblyByName()
        {
        }

        public FilterAssemblyByName(NameCheckTypeEnum nameCheckType, string partialName, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase) :this()
        {
            NameCheckType = nameCheckType;
            PartialName = partialName;
            ComparisonType = comparisonType;
        }

        public bool Filter(Assembly assembly)
        {
            bool isMatch = false;
            var assemblyName = assembly.GetName().FullName;

            switch (NameCheckType)
            {
                case NameCheckTypeEnum.StartsWith:
                    isMatch = assemblyName.StartsWith(PartialName,ComparisonType);
                    break;
                case NameCheckTypeEnum.EndsWith:
                    isMatch = assemblyName.EndsWith(PartialName, ComparisonType);
                    break;
                case NameCheckTypeEnum.Contains:
                    isMatch = assemblyName.IndexOf(PartialName, ComparisonType) !=-1;
                    break;
                case NameCheckTypeEnum.Equals:
                    isMatch = string.Compare(assemblyName, PartialName, ComparisonType) ==0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return isMatch;
        }
    }
}