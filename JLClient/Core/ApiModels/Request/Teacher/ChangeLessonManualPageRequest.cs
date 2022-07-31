using System;

namespace JLClient.Core.ApiModels.Request.Teacher
{
    [Serializable]
    public class ChangeLessonManualPageRequest
    {
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public string NextPage { get; set; }
        public bool ForMainWindows { get; set; }
    }
}
