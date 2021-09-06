using System.Net.Http;

namespace CandyBar.http.workers
{
    public abstract class HttpWorker
    {
        protected readonly HttpClient _client;

        protected HttpWorker(HttpClient client) => _client = client;
    }
}