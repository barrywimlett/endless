namespace Endless.ComponentModel.Validation
{
    public class ValidationError
    {
        public string PropertyName { get; protected set; }
        public string Message { get; protected set; }

        public ValidationError(string propertyName,string message)
        {
            PropertyName = propertyName;
            Message = message;

        }
    }
}