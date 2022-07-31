using JLClient.Core.ApiModels.Request.Auth;
using JLClient.Core.ApiModels.Response.Auth;
using JLClient.Core.Http;
using JLClient.Core.Observer;
using JLClient.Core.Settings;
using JLClient.MVVM.Model.Components.Auth;
using JLClient.MVVM.ViewModel.Components.Auth.Components;
using JLClient.UserControls.Arguments;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace JLClient.MVVM.ViewModel.Components.Auth
{
    public class AuthWindowViewModel : ObservableObject
    {
        // Контексты вложенных окон
        public LoginCardViewModel LoginCardVM { get; set; }

        // События окна
        public event EventHandler OnAuthCompleted;

        // Главный обработчик
        private AuthHandler handler;

        public AuthWindowViewModel()
        {
            handler = new AuthHandler();
            LoginCardVM = new LoginCardViewModel();
            LoginCardVM.OnLoginEnter += LoginCardVM_OnLoginEnter;
        }

        public async void LoginCardVM_OnLoginEnter(object sender, EventArgs e)
        {
            LoginCardVM.IsLoading = true;
            try
            {
                var resp = await handler.LoginCardVM_OnLoginEnter_Async(sender, e);
                OnAuthCompleted?.Invoke(this, null);

            }
            catch(Exception ex)
            {

            }
            finally
            {
                LoginCardVM.IsLoading = false;
            }
        }
    }
}
