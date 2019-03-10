namespace Endless.Composition
{
    public enum ExportPurposeEnum
    {
        None = 0, // because the default value for all enums should mean nothing - so you can spot/debug an unassigned value

        Product,
        Bespoke,
        Experimental,
        UnitTesting
    }
}