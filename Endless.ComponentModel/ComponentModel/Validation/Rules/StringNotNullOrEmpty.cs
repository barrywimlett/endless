namespace Endless.ComponentModel.Validation.Rules
{
    public class StringNotNullOrEmpty : Rule<string>
    {
        public static readonly StringNotNullOrEmpty Singleton = new StringNotNullOrEmpty();

        public StringNotNullOrEmpty() : base ("Missing/Empty String")
        {
            
        }

        public override bool IsValid(string obj)
        {
            return !string.IsNullOrWhiteSpace(obj);
        }
    }
}