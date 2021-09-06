using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SpeakEasy
{
    public class SpeakEasySocketService : BackgroundService
    {
        private readonly ILogger<SpeakEasySocketService> _logger;
        private ClientWebSocket _socket;
        private readonly WebSocketOptions _options;

        public event EventHandler<Message> OnMessageReceived;

        public Uri ConnectionUri { get; set; }

        public SpeakEasySocketService(ILogger<SpeakEasySocketService> logger, IOptions<WebSocketOptions> options)
        {
            _logger = logger;
            _options = options.Value ?? new();
        }

        private void ConfigureSocket()
        {
            if (_options.KeepAliveInterval.TotalSeconds > 0)
            {
                _socket.Options.KeepAliveInterval = _options.KeepAliveInterval;
            }

            if (_options.SocketReceiveBufferSize > 0 && _options.SocketSendBufferSize > 0)
            {
                _socket.Options.SetBuffer(_options.SocketReceiveBufferSize, _options.SocketSendBufferSize);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            _socket = new ClientWebSocket();
            ConfigureSocket();

            _logger.LogInformation($"Attempting SpeakEasy connection to {ConnectionUri}");
            await _socket.ConnectAsync(ConnectionUri, cancellation);
            while (!cancellation.IsCancellationRequested)
            {
                Message msg = await ReceiveAsync(cancellation);
                OnMessageReceived?.Invoke(this, msg);
            }

            _logger.LogInformation("Connection closing");
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellation);
        }

        public async Task SendAsync(byte[] message, WebSocketMessageType messageType, CancellationToken cancellation)
        {
            await _socket.SendAsync(new ArraySegment<byte>(message), messageType, endOfMessage: true, cancellation);
        }

        private async Task<Message> ReceiveAsync(CancellationToken cancellation)
        {
            WebSocketReceiveResult result;

            using MemoryStream ms = new();
            WebSocketMessageType messageType;
            do
            {
                ArraySegment<byte> readBuf = new(new byte[1024]);
                try
                {
                    result = await _socket.ReceiveAsync(readBuf, cancellation);
                }
                catch (Exception e)
                {
                    _logger.LogError($"[SpeakEasy: ERROR] {e}");
                    //If we receive an error while receiving, the websocket will close. Wrap the exception & rethrow for upstreams to handle
                    throw new WebSocketException("Failed to read from websocket", e);
                }

                messageType = result.MessageType;

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    //Close the socket and then invoke the close event to notify subscribers that the socket has been closed
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal Closure", cancellation);
                    await StopAsync(cancellation);
                }

                await ms.WriteAsync(readBuf.Slice(0, result.Count), cancellation);
            } while (!result.EndOfMessage && !cancellation.IsCancellationRequested);

            return new Message(ms.ToArray(), messageType);
        }
    }
}