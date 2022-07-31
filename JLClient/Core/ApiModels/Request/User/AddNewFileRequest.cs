using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class AddNewFileRequest
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
    }
}
