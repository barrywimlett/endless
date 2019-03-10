using System;
using System.Composition;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Attributes
{
    [MetadataAttribute()]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public partial class ExportWithExportPurposeAttribute : ExportAttribute, IExportPurposeMetaData
    {
        public ExportWithExportPurposeAttribute(ExportPurposeEnum purpose, Type exports) :base(exports)
        {
            this.Purpose = purpose;
        }

        public ExportPurposeEnum Purpose { get; protected set; }
    }
}
