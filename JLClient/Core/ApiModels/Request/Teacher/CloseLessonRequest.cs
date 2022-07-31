using System;

namespace JLClient.Core.ApiModels.Request.Teacher
{
    [Serializable]
    public class CloseLessonRequest
    {
        public int CourseId { get; set; }
    }
}
