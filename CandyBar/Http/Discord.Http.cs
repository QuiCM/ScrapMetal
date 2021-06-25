using System;
using System.Net.Http;

namespace CandyBar
{
    public partial class Discord
    {
        public partial class Http
        {
            protected readonly HttpClient _client;

            ///<summary>Generates a new HTTP connector with the given authorization</summary>
            public Http(string authToken)
            {
                _client = new HttpClient { BaseAddress = new Uri("https://discord.com/api/") };
                _client.DefaultRequestHeaders.Add("Authorization", $"Bot {authToken}");
            }
        }
    }
}
