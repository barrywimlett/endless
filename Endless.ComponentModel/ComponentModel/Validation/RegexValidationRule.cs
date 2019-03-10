using System.Text.RegularExpressions;

namespace Endless.ComponentModel.Validation
{
    public class RegexValidationRule : Rule<string>
    {
        private Regex regex = null;
        private RegexOptions RegexOptions { get; set; }
        private string pattern;

        public RegexValidationRule(object error)
            : base(error)
        {
            RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        }

        public string Pattern
        {
            get { return pattern; }
            set
            {
                pattern = value;
                regex = new Regex(pattern, RegexOptions);
            }
        }

        public override bool IsValid(string value)
        {
            // empty strings PASS
            bool isValid = true;
            if (value != null)
            {
                isValid =regex.IsMatch(value.ToString());
            }
            return isValid;
        } 


    }
}