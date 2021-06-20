using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

/*
 SpeakEasy is a simple WebSocket implementation using Microsoft's ClientWebSocket.
 It is built to be as bare-bones as possible, providing a very basic configuration & ignoring many websocket features
 (eg compression, authentication, etc)
*/

namespace SpeakEasy
{
    /// <summary>
    /// A simple wrapper around a <see cref="ClientWebSocket"/>
    /// </summary>
    public class SpeakEasySocket : IDisposable
    {
        private readonly ClientWebSocket _socket;
        private bool disposedValue;

        /// <summary>
        /// <see cref="ClientWebSocket"/> instance used by this SpeakEasySocket
        /// </summary>
        public ClientWebSocket NativeWebSocket => _socket;

        /// <summary>
        /// Event invoked when a complete message is received by the websocket
        /// </summary>
        public event EventHandler<MessageEventArgs> MsgReceived;
        /// <summary>
        /// Event invoked when a close request is received by the websocket
        /// </summary>
        public event EventHandler CloseRequested;
        /// <summary>
        /// Event invoked once the socket has closed
        /// </summary>
        public event EventHandler CloseCompleted;

        /// <summary>
        /// Instantiates a new SpeakEasySocket and its underlying <see cref="ClientWebSocket"/>
        /// </summary>
        public SpeakEasySocket()
        {
            _socket = new ClientWebSocket();
        }

        /// <summary>
        /// Sets the HTTP headers used by the underlying <see cref="ClientWebSocket"/>
        /// </summary>
        /// <param name="headers"></param>
        public void SetHttpHeaders(params ValueTuple<string, string>[] headers)
        {
            foreach (var kvp in headers)
            {
                _socket.Options.SetRequestHeader(kvp.Item1, kvp.Item2);
            }
        }

        /// <summary>
        /// Sets the keep-alive interval of the underlying <see cref="ClientWebSocket"/>
        /// </summary>
        /// <param name="interval"></param>
        public void SetKeepAlive(TimeSpan interval)
        {
            _socket.Options.KeepAliveInterval = interval;
        }

        /// <summary>
        /// Sets the buffer sizes of the underlying <see cref="ClientWebSocket"/>
        /// </summary>
        /// <param name="receiveBufferSize"></param>
        /// <param name="sendBufferSize"></param>
        public void SetBufferSize(int receiveBufferSize, int sendBufferSize)
        {
            _socket.Options.SetBuffer(receiveBufferSize, sendBufferSize);
        }

        /// <summary>
        /// Asynchronously connects to the socket
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ConnectAsync(Uri uri, CancellationToken token)
        {
            await _socket.ConnectAsync(uri, token);
        }

        /// <summary>
        /// Asynchronously receive a message over the socket
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(CancellationToken token)
        {
            WebSocketReceiveResult result;

            using MemoryStream ms = new();
            WebSocketMessageType messageType;
            do
            {
                ArraySegment<byte> readBuf = new(new byte[1024]);
                try
                {
                    result = await _socket.ReceiveAsync(readBuf, token);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"[SpeakEasy: ERROR] {e}");
                    //If we receive an error while receiving, the websocket will close. So invoke the close event to notify upstreams
                    CloseCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }

                messageType = result.MessageType;

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    //Notify that a closure was requested
                    CloseRequested?.Invoke(this, EventArgs.Empty);
                    //Close the socket and then invoke the close event to notify subscribers that the socket has been closed
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close requested by server", token)
                                .ContinueWith(_ => CloseCompleted?.Invoke(this, EventArgs.Empty), token);
                    return;
                }

                await ms.WriteAsync(readBuf.Slice(0, result.Count), token);
            } while (!result.EndOfMessage && !token.IsCancellationRequested);

            MsgReceived?.Invoke(this, new MessageEventArgs(ms.ToArray(), messageType));
        }

        /// <summary>
        /// Asynchronously send a message over the socket
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SendAsync(byte[] message, WebSocketMessageType messageType, CancellationToken token)
        {
            await _socket.SendAsync(new ArraySegment<byte>(message), messageType, endOfMessage: true, token);
        }

        #region Dispose pattern
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _socket.Dispose();
                }

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SpeakEasySocket()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    #endregion
}
