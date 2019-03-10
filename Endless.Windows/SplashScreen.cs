using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Endless.Windows
{
    public class SplashScreen<T,M> where T : System.Windows.Window, new() where M: System.Windows.Window, new()
    {
        // Enhancements:
        // Implement IDisposable
        // If the window is closed in the UI, this class will not know. Could attach to/detach from the close event.
        // reopenable window ?

        // NB this window has its own thread and cant access some WPF bits like resource styles. Everything must be self-contained.

        // In case the caller needs access to the window, eg to call progress update methods on it. Be sure those methods use invoke if it touches the window's UI. 
        public T Window { get; private set; }
        public M MainWindow { get; private set; }

        public void Open()
        {
            if (Window != null)
                return;

            Window = new T();
            Window.Closed += SplashWindowClosed;
            Window.ShowDialog();
            
            /*
            // Manual reset event instance to pause the application till splash screen thread completes its loading. The Window can be used then.
            using (ManualResetEvent manualResetEventSplashScreen = new ManualResetEvent(false))
            {
                Thread splashScreenThread = new Thread(
                    () =>
                    {
                        // Create and install synchronisation context in case window constructor relys on a synchronisation context being there (the default UI thread one wont be).
                        SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(System.Windows.Threading.Dispatcher.CurrentDispatcher));
                        Window = new T();
                        //Window.Closed += (s, e) => System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                        Window.ShowDialog();
                        manualResetEventSplashScreen.Set();
                        System.Windows.Threading.Dispatcher.Run();
                    });
                splashScreenThread.SetApartmentState(ApartmentState.STA);
                splashScreenThread.IsBackground = true;
                splashScreenThread.Name = "Splash Screen Thread";
                splashScreenThread.Start();

                // Wait for the thread to finish.
                manualResetEventSplashScreen.WaitOne();
                
            }
            */
        }

        private void SplashWindowClosed(object sender, EventArgs e)
        {
            MainWindow = new M();
            MainWindow.Show();
        }

        /*
        public void Close()
        {
            
            MainWindow.Show();
            if (Window != null)
            {
                Window.Dispatcher.BeginInvoke(new Action(() => { Window.Close(); Window = null; }));
            }
        }
        */
    }
}
