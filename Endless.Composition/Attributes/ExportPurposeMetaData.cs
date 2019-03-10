using Endless.Composition.Interfaces;

namespace Endless.Composition.Attributes
{
    public partial class ExportPurposeMetaData : IExportPurposeMetaData
    {
        public ExportPurposeEnum Purpose { get; set; }
    }
}