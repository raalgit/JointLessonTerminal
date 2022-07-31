using System;

namespace JLClient.Core.ApiModels.Response.Teacher
{
    [Serializable]
    public class StartSyncLessonResponse : ResponseBase, IResponse
    {
        public bool canConnectToSyncLesson { get; set; }
    }
}
