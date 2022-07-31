using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.Utility.InnerModels
{
    public class UploadFileResponse
    {
        public UploadFileResponse(int fileId, bool isSuccess)
        {
            FileId = fileId;
            IsSuccess = isSuccess;
        }

        public int FileId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
