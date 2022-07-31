using System;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class GetFileResponse : ResponseBase, IResponse
    {
        public byte[] file { get; set; }
    }
}
