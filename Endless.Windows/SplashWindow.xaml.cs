namespace Endless
{
    using System.Windows;
    using System.Windows.Media.Imaging;


    public partial class SplashWindow : MahApps.Metro.Controls.MetroWindow
    {
        public SplashWindow()
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.CacheOption = BitmapCacheOption.OnLoad;
            logo.BeginInit();
            logo.UriSource = new System.Uri("pack://application:,,,/Endless;component/Resources/the-aa-logo.png");
            logo.EndInit();
            DisplayImage.Source = logo;
        }


        public void UpdateProgress(string text)
        {
            // Do the work on this window's own UI thread.
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new System.Action<string>(UpdateMessage), text);
        }


        void UpdateMessage(string text)
        {
            this.Dispatcher.VerifyAccess();
            Message.Text = text;
        }
    }
}
