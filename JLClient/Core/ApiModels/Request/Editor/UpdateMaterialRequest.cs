using JLClient.Core.Manual;
using System;

namespace JLClient.Core.ApiModels.Request.Editor
{
    [Serializable]
    public class UpdateMaterialRequest
    {
        public string OriginalName { get; set; }
        public int OriginalFileDataId { get; set; }
        public byte[] ManualData { get; set; }
    }
}
