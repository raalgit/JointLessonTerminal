using System;

namespace JLClient.Core.ApiModels.Request.Teacher
{
    [Serializable]
    public class StartSyncLessonRequest
    {
        public string StartPage { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
    }
}
