using Flurl.Http;

namespace Oms.Services
{
    public static class HttpService
    {
        static internal Task<T> Post<T>(string url, object? body = null) 
        {
            if (body is null) 
            {
                return url.PostAsync().ReceiveJson<T>();
            }

            return url.PostJsonAsync(body).ReceiveJson<T>();
        }
    }
}
