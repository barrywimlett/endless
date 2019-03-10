using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless
{
    public abstract class ViewModelBase : ModelBase , IViewModelBase
    {
        //protected readonly CurrentDispatcher currentDispatcher= new CurrentDispatcher();

        //protected ILog _log = LogManager.GetLogger(typeof(ViewModelBase));

        public bool IsDebuggerAttached
        {
            get { return Debugger.IsAttached; }
        }

        Task IViewModelBase.AsyncLoad()
        {
            return AsyncLoad();
        }

        protected virtual Task AsyncLoad()
        {
            return null;
        }

    }
}
