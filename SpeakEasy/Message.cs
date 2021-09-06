using System.Net.WebSockets;

namespace SpeakEasy
{
    public class Message
    {
        public byte[] Bytes { get; }
        public WebSocketMessageType MessageType { get; }

        public Message(byte[] bytes, WebSocketMessageType messageType)
        {
            Bytes = bytes;
            MessageType = messageType;
        }
    }
}