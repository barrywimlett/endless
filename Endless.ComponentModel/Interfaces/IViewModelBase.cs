using System.Threading.Tasks;

namespace Endless
{
    
    public interface IViewModelBase :IModelBase
    {
        bool IsDebuggerAttached { get; }

        Task AsyncLoad();

    }
    
}