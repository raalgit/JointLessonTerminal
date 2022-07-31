using JLClient.Core.PersistModels;
using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class GetMyCoursesResponse : ResponseBase, IResponse
    {
        public List<Course> courses { get; set; }
    }
}
