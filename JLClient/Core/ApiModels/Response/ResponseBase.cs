namespace JLClient.Core.ApiModels.Response
{
    public class ResponseBase : IResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public bool showMessage { get; set; }
    }
}
