using JLClient.Core.ApiModels.Request.Teacher;
using JLClient.Core.ApiModels.Response.Teacher;
using JLClient.Core.ApiModels.Response.User;
using JLClient.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Course
{
    public class CourseDescriptionhandler
    {
        public async Task<bool> StopLesson(int courseId)
        {
            var closeLessonRequest = new RequestModel<CloseLessonRequest>()
            {
                Method = RequestMethod.Post,
                Body = new CloseLessonRequest()
                {
                    CourseId = courseId
                }
            };

            var sender = new RequestSender<CloseLessonRequest, CloseLessonResponse>();
            var responsePost = await sender.SendRequest(closeLessonRequest, "/teacher/close-sync-lesson");
            if (responsePost.isSuccess) return !responsePost.canConnectToSyncLesson;
            return false;
        }

        public async Task<bool> StartLesson(int courseId, int groupId, string page = null)
        {
            var startLessonRequest = new RequestModel<StartSyncLessonRequest>()
            {
                Method = RequestMethod.Post,
                Body = new StartSyncLessonRequest()
                {
                    StartPage = page ?? String.Empty,
                    CourseId = courseId, 
                    GroupId = 1
                }
            };

            var sender = new RequestSender<StartSyncLessonRequest, StartSyncLessonResponse>();
            var responsePost = await sender.SendRequest(startLessonRequest, "/teacher/start-sync-lesson");
            if (responsePost.isSuccess) return responsePost.canConnectToSyncLesson;
            return false;
        }

        public async Task<GetCourseDataResponse> LoadCourseData(int courseId)
        {
            var manualGetRequest = new RequestModel<object>()
            {
                Method = RequestMethod.Get,
                UrlFilter = $"/{courseId}"
            };
            var sender = new RequestSender<object, GetCourseDataResponse>();
            var responsePost = await sender.SendRequest(manualGetRequest, "/user/course-data");
            return responsePost;
        }
    }
}
