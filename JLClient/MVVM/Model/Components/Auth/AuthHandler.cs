using JLClient.Core.ApiModels.Request.Auth;
using JLClient.Core.ApiModels.Response.Auth;
using JLClient.Core.Http;
using JLClient.Core.Settings;
using JLClient.UserControls.Arguments;
using System;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Auth
{
    public class AuthHandler
    {
        public async Task<LoginResponse> LoginCardVM_OnLoginEnter_Async(object sender, EventArgs e)
        {
            var data = getArgData(e);
            if (data == null) throw new ArgumentNullException("Не удалось получить пару Логин/Пароль");
            if (!LoginDataIsValid(data)) throw new ArgumentException("Логин/пароль не установлен(ы)");

            var loginRequest = new RequestModel<LoginRequest>()
            {
                Method = RequestMethod.Post,
                Body = new LoginRequest()
                {
                    Login = data.Login,
                    Password = data.Password
                },
                UseCurrentToken = false
            };

            try
            {
                var resp = await Login(loginRequest);
                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LoginEventArg getArgData(object arg)
        {
            if (arg is LoginEventArg data)
                return data;

            return null;
        }

        private bool LoginDataIsValid(LoginEventArg data)
        {
            if (string.IsNullOrEmpty(data.Login) || string.IsNullOrEmpty(data.Password))
            {
                return false;
            }

            return true;
        }

        private async Task<LoginResponse> Login(RequestModel<LoginRequest> loginRequest)
        {
            try
            {
                var httpSender = new RequestSender<LoginRequest, LoginResponse>();
                var responsePost = await httpSender.SendRequest(loginRequest, "/auth/login");

                if (responsePost.isSuccess)
                {
                    var settings = UserSettings.GetInstance();
                    settings.JWT = responsePost.jwt;
                    settings.CurrentUser = responsePost.user;
                    settings.Roles = responsePost.roles.ToArray();
                }
                else
                {
                    throw new Exception(responsePost.message);
                }

                return responsePost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
