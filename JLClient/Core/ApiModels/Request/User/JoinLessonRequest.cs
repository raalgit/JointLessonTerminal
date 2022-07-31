using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class JoinLessonRequest
    {
        public int CourseId { get; set; }
    }
}
