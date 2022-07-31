using JLClient.Core.Manual;
using System;

namespace JLClient.Core.ApiModels.Request.Editor
{
    [Serializable]
    public class NewMaterialRequest
    {
        public string OriginalName { get; set; }
        public byte[] ManualData { get; set; }
    }
}
