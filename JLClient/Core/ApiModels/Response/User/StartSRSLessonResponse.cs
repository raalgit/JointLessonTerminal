using System;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class StartSRSLessonResponse : ResponseBase, IResponse
    {
        public string page { get; set; }
    }
}
