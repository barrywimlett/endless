namespace Endless.ComponentModel.Validation.Rules
{
    public class NotNullRule<T> : Rule<T> where T:class 
    {

        public NotNullRule()
            : base("Must not be null")
        {
            
        }

        public override bool IsValid(T obj)
        {
            bool isValid = obj!=null;
            return isValid;
            
        }
    }
}