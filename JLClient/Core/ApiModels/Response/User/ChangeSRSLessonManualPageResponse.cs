using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.Core.ApiModels.Response.User
{
    [Serializable]
    public class ChangeSRSLessonManualPageResponse : ResponseBase, IResponse
    {
        public string newPage { get; set; }
    }
}
