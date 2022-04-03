using Flurl.Http;

namespace Oms.Services
{
    public static class HttpService
    {
        public static Task<T> Post<T>(string url, object? body = null)
        {
            return body is null ? url.PostAsync().ReceiveJson<T>() : url.PostJsonAsync(body).ReceiveJson<T>();
        }
    }
}