using System.Collections.Generic;

namespace Endless.ComponentModel.Validation
{
    public interface IRuleBinding
    {
        bool IsValid { get; }
        object Error { get; }
        List<string> MappedProperties { get; }
    }
}