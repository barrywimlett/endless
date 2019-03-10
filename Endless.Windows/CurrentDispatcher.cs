using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Endless.Windows
{
    public class CurrentDispatcher
    {
        private Dispatcher _dispatcher;

        public CurrentDispatcher()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void Invoke(Action action)
        {
            if (_dispatcher.CheckAccess() == false)
            {
                _dispatcher.Invoke(action);
            }
            else
            {
                action();
            }

        }

        public Task BeginInvoke(Action action)
        {
            return _dispatcher.BeginInvoke(action).Task;
        }

        public  T Evaluate<T>(Func<T> action) {
            T result;

            if (_dispatcher.CheckAccess() == false)
            {
                result = _dispatcher.Invoke(action);
            }
            else
            {
                result = action();
            }

            return result;
        }

    }
}