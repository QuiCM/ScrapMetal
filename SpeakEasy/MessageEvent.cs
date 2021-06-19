using System;
using System.Collections.Generic;

using System.Net.WebSockets;

namespace SpeakEasy
{
    public class MessageEventArgs : EventArgs
    {
        public byte[] Bytes { get; }
        public WebSocketMessageType MessageType { get; }

        public MessageEventArgs(byte[] bytes, WebSocketMessageType messageType)
        {
            Bytes = bytes;
            MessageType = messageType;
        }
    }
}