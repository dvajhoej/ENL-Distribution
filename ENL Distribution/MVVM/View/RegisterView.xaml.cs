using ENL_Distribution.CustomControle;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ENL_Distribution.MVVM.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection("Server=(localdb)\\local; Database=LRDATABASE; Integrated Security=True"))
            {
                con.Open();

                string add_data = "INSERT INTO [dbo].[user] ([username], [password]) VALUES (@username, @password)";
                using (SqlCommand cmd = new SqlCommand(add_data, con))
                {
                    
                    cmd.Parameters.AddWithValue("@username", txtNewUser.Text);
                    cmd.Parameters.AddWithValue("@password", Password.Password);

                    cmd.ExecuteNonQuery();
                }
            
                con.Close();
                txtNewUser.Text = "";
                Password.Password = "";
                var loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
           
        }




    }

}

