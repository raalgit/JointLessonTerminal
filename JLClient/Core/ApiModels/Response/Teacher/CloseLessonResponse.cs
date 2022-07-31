using System;

namespace JLClient.Core.ApiModels.Response.Teacher
{
    [Serializable]
    public class CloseLessonResponse : ResponseBase, IResponse
    {
        public bool canConnectToSyncLesson { get; set; }
    }
}
