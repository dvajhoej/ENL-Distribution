using ENL_Distribution.MVVM.View;
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
            var LoginView = new LoginView();
            LoginView.Show();
            LoginView.IsVisibleChanged += (s, ev) =>
            {
                if (LoginView.IsVisible == false && LoginView.IsLoaded)
                {
                    var MainWindow = new MainWindow();
                    MainWindow.Show();
                    LoginView.Close();
                }
            };
        }
    }

}
