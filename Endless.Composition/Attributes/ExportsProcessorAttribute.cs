using System;
using System.Composition;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Attributes
{
    [MetadataAttribute]
    public class ExportsProcessorAttribute : ExportAttribute, IProcessorMetaData
    {
        public int EventId { get; protected set; }
        ExportsProcessorAttribute(int eventid,Type exports) : base(exports)
        {
            this.EventId = eventid;
        }
    }
}