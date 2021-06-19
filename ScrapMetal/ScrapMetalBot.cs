using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar;
using CandyBar.models;
using ScrapMetal.Configuration;
using SpeakEasy;

/*
    ScrapMetal is a Discord Bot built on CandyBar and SpeakEasy
*/
namespace ScrapMetal
{
    public class ScrapMetalBot : IDisposable
    {
        internal readonly SpeakEasySocket _websocket;
        internal ScrapMetalBrain _brain;
        internal ScrapMetalHeart _heart;
        internal RuntimeConfiguration _configuration;

        private readonly ScrapMetalPersistentConfig _persistentConfig;
        private readonly Discord.Http _http;
        private readonly CancellationTokenSource _tokenSource;

        private bool disposedValue;

        public event EventHandler CloseRequested;

        public ScrapMetalBrain Brain => _brain;

        internal ScrapMetalBot(BuilderOptions options)
        {
            Debug.WriteLine("Building a new ScrapMetalBot.");

            _tokenSource = options._tokenSource ?? new();
            _persistentConfig = PersistentConfiguration.Load<ScrapMetalPersistentConfig>();
            _configuration = RuntimeConfiguration.LoadFrom(_persistentConfig);

            if (options._auth != null)
            {
                //Override configured auth with a runtime-provided auth
                _configuration.AuthToken.Value = options._auth;
            }

            _http = new Discord.Http(_configuration.AuthToken.Value);
            _brain = options._brain ?? new();
            _brain.UpdateConfiguration(_configuration);
            _brain.UpdateScrapMetal(this);

            _websocket = new SpeakEasySocket();
            _websocket.SetBufferSize(options._recvBufSize, options._sendBufSize);

            _websocket.MsgReceived += OnMessageReceived;
            _websocket.CloseRequested += OnSpeakEasyCloseRequested;
            _websocket.CloseCompleted += OnSpeakEasyClosed;
        }

        /// <summary>Asynchronously attempts to connect ScrapMetal to the Discord Gateway</summary>
        public async Task ConnectAsync()
        {
            string url = await _http.GetGatewayUrl(_tokenSource.Token);
            await _websocket.ConnectAsync(new Uri(url), _tokenSource.Token);

            Debug.WriteLine("SpeakEasy now speaking easy.");
        }

        public async Task SendAsync(string message)
        {
            Debug.WriteLine($"Sending payload: {message}.");
            await _websocket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, _tokenSource.Token);
        }

        ///<summary>
        ///Polls the underlying websocket until ScrapMetal's cancellation token is cancelled
        ///</summary>
        public async Task PollAsync()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                await _websocket.ReceiveAsync(_tokenSource.Token);
            }
        }

        public void Close()
        {
            _persistentConfig.Write();
            //If we receive a close request, cancel our token. This should cause everything else to stop
            _tokenSource.Cancel();
        }

        private async void OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.MessageType == WebSocketMessageType.Binary)
            {
                //We don't want to handle binary because we are lazy.
                return;
            }

            gateway_payload payload = JsonSerializer.Deserialize<gateway_payload>(e.Bytes);
            _brain._sequence = payload.s;

            Debug.WriteLine($"Payload received: [{payload.s}] {payload.op} | {payload.t} | {payload.d}.");

            if (payload.op == 10)
            {
                _heart = new ScrapMetalHeart(this);
                await _heart.FirstBeat(payload.d.GetProperty("heartbeat_interval").GetInt32(), _tokenSource.Token);
                _heart.Beat();

                await _brain.HandleGatewayConnect();
            }
            else if (payload.op == 0 && payload.t == "READY")
            {
                _brain.HandleReadyEvent(payload);
            }
            else if (payload.op == 9)
            {
                await _brain.HandleGatewayConnect(reidentify: true);
            }
        }

        private void OnSpeakEasyClosed(object sender, EventArgs e)
        {
            Close();
        }

        private void OnSpeakEasyCloseRequested(object sender, EventArgs e)
        {
            Debug.WriteLine("Conversation over. SpeakEasy shutting down.");
            CloseRequested?.Invoke(sender, e);
        }

        #region Dispose pattern
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _websocket.Dispose();
                }

                if (!_tokenSource.IsCancellationRequested)
                {
                    _tokenSource.Cancel();
                }

                _configuration = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    #endregion
}