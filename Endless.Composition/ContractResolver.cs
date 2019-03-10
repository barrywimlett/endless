using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

namespace Endless.Composition.Config
{
    public class ContractResolver : DataContractResolver 
    {

        public static ContractResolver singleton = new ContractResolver();
        public class Registration 
        {
            internal string TypeName { get; private set; }
            internal string TypeNamespace { get; private set; }
            internal Type Type { get; private set; }

            
            public Registration(string typeName, string typeNamespace, Type type)
            {
                this.TypeName = typeName;
                this.TypeNamespace = typeNamespace;
                this.Type = type;
            }
        }

        readonly private List<Registration> registrations= new List<Registration>();

        /// <summary>
        /// use the singleton instance
        /// </summary>
        protected ContractResolver()
        {
            
        }
        public void Register(string typeName, string typeNamespace, Type type)
        {
            var existing = FindRegistration(typeName, typeNamespace);
            if (existing != null)
            {
                if (type != existing.Type)
                {
                    throw new ApplicationException($"Attempted duplicate registration with typename:${typeName} typeNamespace:{typeNamespace} existing type:{existing.Type} new Type:{type}");
                }
            }
            else
            {
                var newRegistration = new Registration(typeName, typeNamespace, type);
                registrations.Add(newRegistration);
            }
        }

        protected Registration FindRegistration(string typeName, string typeNamespace)
        {
            return registrations.FirstOrDefault(r => r.TypeName == typeName && r.TypeNamespace == typeNamespace);
        }

        protected Registration FindRegistration(Type t)
        {
            return registrations.FirstOrDefault(r => r.Type==t);
        }

        public void RegisterContractsByReflection(Assembly assembly )
        {
            foreach (var t in assembly.GetTypes())
            {
                var attrs =
                    t.GetCustomAttributes(typeof(System.Runtime.Serialization.DataContractAttribute))
                        .Cast<System.Runtime.Serialization.DataContractAttribute>();

                foreach (var attr in attrs)
                {
                    string typeName = (attr.IsNameSetExplicitly) ? attr.Name : t.Name;
                    string typeNameSpace = (attr.IsNamespaceSetExplicitly) ?   attr.Namespace : t.Namespace;

                    Register(typeName,typeNameSpace,t);
                }
            }
        }

        override public bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            var existing = FindRegistration(dataContractType);

            if (existing!=null)
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add(existing.TypeName);
                typeNamespace = dictionary.Add(existing.TypeNamespace);
                return true;
            }
            else
            {
                return knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace);
            }
        }

        //public abstract Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver);
        override public Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            var existing = FindRegistration(typeName,typeNamespace);

            if (existing!=null)
            {
                return existing.Type;
            }
            else
            {
                return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
            }
        }
    }
}