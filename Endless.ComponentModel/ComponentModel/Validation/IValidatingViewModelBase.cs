using System.Collections.Generic;
using System.ComponentModel;

namespace Endless.ComponentModel.Validation
{
    public interface IValidatingViewModelBase : IViewModelBase, INotifyDataErrorInfo
    {
        bool DisableValidation { get; set; }
        List<ValidationError> ValidationErrors { get; }
    }
}