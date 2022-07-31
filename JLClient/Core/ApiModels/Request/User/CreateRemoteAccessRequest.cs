using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class CreateRemoteAccessRequest
    {
        public string ConnectionData { get; set; }
        public int CourseId { get; set; }
    }
}
