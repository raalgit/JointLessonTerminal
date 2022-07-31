using System;
using System.Collections.Generic;

namespace JLClient.Core.ApiModels.Response.Editor
{
    [Serializable]
    public class GetMyMaterialsResponse : ResponseBase, IResponse
    {
        public List<JLClient.Core.PersistModels.Manual> manuals { get; set; }
    }
}
