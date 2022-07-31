using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class LoadNoteRequest
    {
        public int CourseId { get; set; }
    }
}
