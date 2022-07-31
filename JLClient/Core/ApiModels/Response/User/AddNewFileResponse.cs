using System;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class AddNewFileResponse : ResponseBase, IResponse
    {
        public int fileDataId { get; set; }
    }
}
