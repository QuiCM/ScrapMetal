using System.Threading;

namespace ScrapMetal
{
    internal class BuilderOptions
    {
        //Note: 16*1024 is the default receive buffer size for a ClientWebSocket instance.
        //Discord will only accept sends of up to 4096 bytes.
        internal int _recvBufSize = 16 * 1024, _sendBufSize = 4096;
        internal string _auth;
        internal CancellationTokenSource _tokenSource;
        internal ScrapMetalBrain _brain;
    }
}