using JLClient.Core.ApiModels.Response.User;
using JLClient.Core.Http;
using JLClient.MVVM.Model.Components.Profile.InnerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Profile
{
    public class MyCourseCardHandler
    {
        public async Task<List<CourseData>> GetMyCourses()
        {
            var getCoursesRequest = new RequestModel<object>()
            {
                Method = RequestMethod.Get
            };

            var sender = new RequestSender<object, GetMyCoursesResponse>();
            var responseGet = await sender.SendRequest(getCoursesRequest, "/user/my-courses");
            if (responseGet.isSuccess)
            {
                var resp = responseGet.courses.Select(x => new CourseData()
                {
                    Description = x.description,
                    Id = x.id,
                    DisciplineId = x.disciplineId,
                    ManualId = x.manualId,
                    Name = x.name

                }).ToList();
                return resp;
            }

            throw new InvalidOperationException();
        }
    }
}
