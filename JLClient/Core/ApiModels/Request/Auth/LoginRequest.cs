using System;

namespace JLClient.Core.ApiModels.Request.Auth
{
    [Serializable]
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
