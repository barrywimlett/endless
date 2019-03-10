namespace Endless.ComponentModel.Validation.Rules
{
    public class MinimumLengthString : Rule<string>
    {
        
        private int minLength;
        public MinimumLengthString(int minLength)
            : base("String is not long enough")
        {
            this.minLength = minLength;
        }

        public override bool IsValid(string obj)
        {
            bool isValid = false;
            string str = obj as string;
            if (str != null && str.Trim().Length>=minLength)
            {
                isValid = true;
            }
            return isValid;
        }
    }
}