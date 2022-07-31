using System;
using System.Text.Json;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JLClient.Core.Settings;
using JLClient.Core.Notifications;
using JLClient.Core.ApiModels.Response;

namespace JLClient.Core.Http
{
    public class RequestSender<TReq, TRes>
    {
        /// <summary>
        /// Отправка HTTP запроса на сервера
        /// </summary>
        /// <param name="requestModel">Данные запроса</param>
        /// <param name="route">адрес api</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TRes> SendRequest(RequestModel<TReq> requestModel, string route)
        {
            TRes response = default;

            // Получение полного адреса нужного api 
            string serverUrl = ConfigurationManager.AppSettings["ServerUrl"].ToString();
            serverUrl += route;
            if (!string.IsNullOrEmpty(requestModel.UrlFilter)) serverUrl += requestModel.UrlFilter;
            var uri = new Uri(serverUrl);

            var result = (HttpResponseMessage)null;
            var requestParams = (HttpRequestMessage)null;

            using (var httpclient = new HttpClient())
            {
                // Создание HTTP запроса
                requestParams = new HttpRequestMessage();
                requestParams.RequestUri = uri;

                var settings = UserSettings.GetInstance();
                if (!string.IsNullOrEmpty(settings.JWT) && requestModel.UseCurrentToken) 
                    requestParams.Headers.Add("Authorization", "Bearer " + settings.JWT);

                // Проверка типа запроса
                switch (requestModel.Method)
                {
                    case RequestMethod.Post:
                        requestParams.Method = HttpMethod.Post;
                        string jsonPost = JsonSerializer.Serialize<TReq>(requestModel.Body);
                        var contentPost = new StringContent(jsonPost, Encoding.UTF8, "application/json");
                        requestParams.Content = contentPost;
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    case RequestMethod.Get:
                        requestParams.Method = HttpMethod.Get;
                        requestParams.Headers.Add("Accept", "text/plain");
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    case RequestMethod.Delete:
                        requestParams.Method = HttpMethod.Delete;
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    case RequestMethod.Put:
                        requestParams.Method = HttpMethod.Put;
                        string jsonPut = JsonSerializer.Serialize<TReq>(requestModel.Body);
                        var contentPut = new StringContent(jsonPut, Encoding.UTF8, "application/json");
                        requestParams.Content = contentPut;
                        result = await httpclient.SendAsync(requestParams);
                        break;

                    default:
                        throw new ArgumentException(nameof(requestModel.Method));
                }
            }

            using (var streamResponse = await result.Content.ReadAsStreamAsync())
            {
                if (streamResponse != null)
                {
                    var responseStreamReader = new StreamReader(streamResponse, Encoding.UTF8);
                    var responseStr = responseStreamReader.ReadToEnd();
                    response = JsonSerializer.Deserialize<TRes>(responseStr);
                }
            }

            result.EnsureSuccessStatusCode();

            requestParams.Dispose();
            result.Dispose();

            var respData = response as IResponse;
            if (respData.showMessage)
            {
                var toastManager = new SystemToastManager();
                toastManager.SendToast(respData.message);
            }

            return response;
        }
    }
}
