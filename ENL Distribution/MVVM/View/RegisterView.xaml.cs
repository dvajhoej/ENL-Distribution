using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

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
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

            if (txtNewUser.Text.Length >= 5 && TxtName.Text.Length >= 2 && TxtLastName.Text.Length >= 1 && txtNewEmail.Text.Contains("@"))
            {


                using (SqlConnection con = new SqlConnection("Server=(localdb)\\local; Database=MVVMLoginDb; Integrated Security=True"))
                {
                    con.Open();

                    string add_data = "INSERT INTO [dbo].[user] ([Username], [Name], [LastName], [Password], [Email]) VALUES (@Username, @Name, @LastName, @Password, @Email)";
                    using (SqlCommand cmd = new SqlCommand(add_data, con))
                    {

                        cmd.Parameters.AddWithValue("@Username", txtNewUser.Text);
                        cmd.Parameters.AddWithValue("@Password", Password.Password);
                        cmd.Parameters.AddWithValue("@Name", TxtName.Text);
                        cmd.Parameters.AddWithValue("@LastName", TxtLastName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtNewEmail.Text);


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
            else
            {
                Errorbox.Text = "Indtast Korrekte Oplysninger";
                Password.Password = "";
            }
        }

 
    }

}

