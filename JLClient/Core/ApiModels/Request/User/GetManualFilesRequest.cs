using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Request.User
{
    [Serializable]
    public class GetManualFilesRequest
    {
        public List<int> FileDataIds { get; set; }
    }
}
