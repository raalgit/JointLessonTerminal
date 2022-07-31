using JLClient.Core.PersistModels;
using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class GetRemoteAccessListResponse : ResponseBase, IResponse
    {
        public List<UserRemoteAccessWithUserData> userRemoteAccesses { get; set; }
    }

    [Serializable]
    public class UserRemoteAccessWithUserData
    {
        public UserRemoteAccess userRemote { get; set; }
        public string userName { get; set; }
    }
}
