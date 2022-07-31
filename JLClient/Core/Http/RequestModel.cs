
namespace JLClient.Core.Http
{
    public class RequestModel<TReq>
    {
        public RequestMethod Method { get; set; }
        public TReq Body { get; set; }
        public string UrlFilter { get; set; }
        public bool UseCurrentToken { get; set; } = true;
        public bool UploadFile { get; set; } = false;
    }
}
