using ENL_Distribution.MVVM.Model;
using ENL_Distribution.Repositories;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Windows.Input;

namespace ENL_Distribution.MVVM.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository; 

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                onPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                onPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                onPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                onPropertyChanged(nameof(IsViewVisible));
            }
        }
        // Commands
        public ICommand LoginCommand {  get; }
        public ICommand RecoveryPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        // Constructors
        public LoginViewModel()
        {
            userRepository= new UserRepository();
            LoginCommand = new ViewModelCommands(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoveryPasswordCommand = new ViewModelCommands(p => ExecuteRecoverPassCommand("", ""));
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validdata;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3)
                validdata = false;
            else
                validdata = true;
            return validdata;
        }
   
        private void ExecuteLoginCommand(Object obj)
        {
            var IsUserValid = userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            if (IsUserValid)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;
                var mainView = new MainWindow();
                mainView.Show();
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }
        private void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
