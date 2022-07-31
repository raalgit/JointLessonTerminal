using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class GetRemoteAccessDataRequest
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
