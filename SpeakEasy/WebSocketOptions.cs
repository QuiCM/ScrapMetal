using System;
using System.Collections.Generic;

namespace SpeakEasy
{
    public class WebSocketOptions
    {
        public TimeSpan KeepAliveInterval { get; set; }
        //Note: 16*1024 is the default receive buffer size for a ClientWebSocket instance.
        //Discord will only accept send sizes of up to 4kb.
        public int SocketReceiveBufferSize { get; set; } = 16 * 1024;
        public int SocketSendBufferSize { get; set; } = 4 * 1024;
    }
}