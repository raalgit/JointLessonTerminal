using JLClient.Core.PersistModels;
using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Response.Auth
{
    [Serializable]
    public class LoginResponse : ResponseBase, IResponse
    {
        public JLClient.Core.PersistModels.User user { get; set; }
        public List<Role> roles { get; set; }
        public string jwt { get; set; }
    }
}
