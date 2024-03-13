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
            var RegView = new RegisterView();
            
            // Subscribe to the Closed event of RegView
            RegView.Closed += (s, args) =>
            {
                // Check if RegView is not visible and loginView is loaded
                if (!RegView.IsVisible && loginView.IsLoaded)
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                }
                else
                {
                    loginView.Show();
                }
            };

            RegView.Close();
        }
    }
}