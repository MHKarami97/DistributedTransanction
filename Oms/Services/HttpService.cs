using Flurl.Http;

namespace Oms.Services
{
    public static class HttpService
    {
        public static Task<T> Post<T>(string url, object? body = null,
            CancellationToken cancellationToken = new())
        {
            return body is null
                ? url.PostAsync(cancellationToken: cancellationToken).ReceiveJson<T>()
                : url.PostJsonAsync(body, cancellationToken).ReceiveJson<T>();
        }
    }
}