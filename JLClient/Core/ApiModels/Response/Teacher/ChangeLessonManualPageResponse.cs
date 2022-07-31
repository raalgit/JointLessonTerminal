using System;

namespace JLClient.Core.ApiModels.Response.Teacher
{
    [Serializable]
    public class ChangeLessonManualPageResponse : ResponseBase, IResponse
    {
        public string newPage { get; set; }
        public string isOnline { get; set; }
    }
}
