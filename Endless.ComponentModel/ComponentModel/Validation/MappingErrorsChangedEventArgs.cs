namespace Endless.ComponentModel.Validation
{
    public class MappingErrorsChangedEventArgs
    {
        public string PropertyName { get; protected set; }
        public bool RequiresRevalidate { get; protected set; }

        public MappingErrorsChangedEventArgs(string propertyName, bool requiresRevalidate)
        {
            this.PropertyName = propertyName;
            this.RequiresRevalidate = requiresRevalidate;
        }
    }
}
