using Endless.Composition.Attributes;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Test
{
    [ExportWithExportPurpose(ExportPurposeEnum.Product,typeof(ITestInterface))]
    public class TestProductImplimentation : ITestInterface
    {
        
    }
}