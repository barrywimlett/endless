namespace Endless.ComponentModel.Validation.Rules
{
    public class UKPostcodeValidationRule : RegexValidationRule
    {
        public static readonly UKPostcodeValidationRule Singleton =new UKPostcodeValidationRule();

        public UKPostcodeValidationRule() : base("Not a valid UK postcode")
        {
            Pattern =
                @"^((([A-PR-UWYZ][A-HK-Y]?[0-9][0-9]?)|(([A-PR-UWYZ][0-9][A-HJKSTUW])|([A-PR-UWYZ][A-HK-Y][0-9][ABEHMNPRV-Y])))\s*[0-9][ABD-HJLNP-UW-Z]{2})?$";
        }
#if DEBUG
        // only used for breakpoint
        public override bool IsValid(string value)
        {
                return base.IsValid(value);
        }
#endif
    }
}