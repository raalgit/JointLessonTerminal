using System;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class GetRemoteAccessDataResponse : ResponseBase, IResponse
    {
        public string connectionData { get; set; }
    }
}
