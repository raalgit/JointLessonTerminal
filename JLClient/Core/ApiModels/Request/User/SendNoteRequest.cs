using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class SendNoteRequest
    {
        public byte[] File { get; set; }
        public int CourseId { get; set; }
    }
}
