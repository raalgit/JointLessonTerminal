using JLClient.Core.Manual;
using System;

namespace JLClient.Core.ApiModels.Response.Editor
{
    [Serializable]
    public class GetMaterialResponse : ResponseBase, IResponse
    {
        public string originalName { get; set; }
        public byte[] manualData { get; set; }
    }
}
