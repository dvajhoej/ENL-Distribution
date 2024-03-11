using ENL_Distribution.MVVM.View;
using Microsoft.Win32;
using System.Windows;

namespace ENL_Distribution
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
        }
    }

}
