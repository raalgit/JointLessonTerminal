using System;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    internal class ChangeSRSLessonManualPageRequest
    {
        public int CourseId { get; set; }
        public string NextPage { get; set; }
    }
}
