using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class LeaveLessonRequest
    {
        public int CourseId { get; set; }
    }
}
