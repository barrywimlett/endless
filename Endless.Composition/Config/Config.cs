using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Endless.Composition.Config
{
    //[Serializable]
    [DataContract(Namespace = "Endless.Composition.Config")]
    public class Config
    {
        public Config()
        {
            this.Sources= new List<AssemblySourceConfigBase>();
            this.Includes = new List<AssemblyFilterConfigBase>();
            this.Excludes= new List<AssemblyFilterConfigBase>();
        }

        [DataMember]
        public List<AssemblySourceConfigBase> Sources { get; set; }

        [DataMember]
        public List<AssemblyFilterConfigBase> Includes { get; set; }

        [DataMember]
        public List<AssemblyFilterConfigBase> Excludes { get; set; }

    }
}