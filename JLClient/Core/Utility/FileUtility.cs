using JLClient.Core.ApiModels.Request.User;
using JLClient.Core.ApiModels.Response.User;
using JLClient.Core.Http;
using JLClient.Core.Utility.InnerModels;
using System.Threading.Tasks;

namespace JLClient.Core.Utility
{
    public class FileUtility
    {
        public async Task<UploadFileResponse> UploadFile(byte[] file, string name)
        {
            UploadFileResponse response = null;

            var fileSaveRequest = new RequestModel<AddNewFileRequest>()
            {
                Method = RequestMethod.Post,
                Body = new AddNewFileRequest()
                {
                    File = file,
                    Name = name
                },
            };

            var sender = new RequestSender<AddNewFileRequest, AddNewFileResponse>();
            var responsePost = await sender.SendRequest(fileSaveRequest, "/user/file");
            response = new UploadFileResponse(responsePost.fileDataId, responsePost.isSuccess);
            return response;
        }

        public async Task<GetFileResponse> DownloadFile(int fileDataId)
        {
            var fileGetRequest = new RequestModel<object>()
            {
                Method = RequestMethod.Get,
                UrlFilter = $"/{fileDataId}"
            };
            var sender = new RequestSender<object, GetFileResponse>();
            return await sender.SendRequest(fileGetRequest, "/user/file");
        }
    }
}
