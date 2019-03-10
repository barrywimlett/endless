namespace Endless.ComponentModel.Validation.Rules
{
    public class PhoneNumberValidationRule : RegexValidationRule
    {
        public static readonly PhoneNumberValidationRule Singleton = new PhoneNumberValidationRule();

        public PhoneNumberValidationRule()
            : base("Not a valid phonenumber")
        {
            Pattern =
                @"^(\(?0[0-9\(\)\s\-\+]{10,20})?$";
        }
    }
}