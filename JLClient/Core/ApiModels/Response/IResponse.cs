
namespace JLClient.Core.ApiModels.Response
{
    public interface IResponse
    {
        bool isSuccess { get; set; }
        string message { get; set; }
        bool showMessage { get; set; }
    }
}
