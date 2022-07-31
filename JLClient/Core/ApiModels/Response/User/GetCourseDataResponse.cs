using JLClient.Core.PersistModels;
using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class GetCourseDataResponse : ResponseBase, IResponse
    {
        public List<CourseTeacher> courseTeachers { get; set; }
        public List<Group> courseGroups { get; set; }
        public bool isTeacher { get; set; }
        public bool lessonIsActive { get; set; }
    }
}
