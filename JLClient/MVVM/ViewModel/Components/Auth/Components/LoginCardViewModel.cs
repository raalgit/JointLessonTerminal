using JLClient.Core.Observer;
using JLClient.UserControls.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace JLClient.MVVM.ViewModel.Components.Auth.Components
{
    public class LoginCardViewModel : ObservableObject
    {
        public event EventHandler OnLoginEnter;
        
        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;
        public string Login { get { return login; } set { login = value; OnPropsChanged("Login"); } }
        private string login;
        public string Password { get { return password; } set { password = value; OnPropsChanged("Password"); } }
        private string password;

        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; OnPropsChanged("ErrorMessage"); } }
        private string errorMessage;

        public RelayCommand SendLoginData { get; set; }

        public LoginCardViewModel()
        {
            IsLoading = false;
            SendLoginData = new RelayCommand((arg) => {
                if (string.IsNullOrEmpty(this.Login) || string.IsNullOrEmpty(this.Password))
                {
                    ErrorMessage = "Пожалуйста, введите логин/пароль!";
                    return;
                }

                ErrorMessage = "";
                OnLoginEnter?.Invoke(this, new LoginEventArg()
                {
                    Login = this.Login,
                    Password = this.Password
                });
            });
        }
    }
}
