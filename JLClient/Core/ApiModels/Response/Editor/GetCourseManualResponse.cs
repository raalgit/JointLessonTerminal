using System;

namespace JLClient.Core.ApiModels.Response.Editor
{
    [Serializable]
    public class GetCourseManualResponse : ResponseBase, IResponse
    {
        public JLClient.Core.PersistModels.Manual manual { get; set; }
    }
}
