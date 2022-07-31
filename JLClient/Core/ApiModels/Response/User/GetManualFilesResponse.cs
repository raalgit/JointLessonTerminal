using JLClient.Core.PersistModels;
using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class GetManualFilesResponse : ResponseBase, IResponse
    {
        public List<FileData> fileDatas { get; set; }
    }
}
