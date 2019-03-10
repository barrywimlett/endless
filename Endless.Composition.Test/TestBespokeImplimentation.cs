using Endless.Composition.Attributes;
using Endless.Composition.Interfaces;

namespace Endless.Composition.Test
{
    [ExportWithExportPurpose(ExportPurposeEnum.Bespoke, typeof(ITestInterface))]
    public class TestBespokeImplimentation : ITestInterface
    {

    }
}