

using System.Threading;

namespace ScrapMetal
{
    /// <summary>
    /// Builder for a ScrapMetalBot
    /// </summary>
    public class ScrapMetalBuilder
    {
        private readonly BuilderOptions _options;

        /// <summary>
        /// Instantiates a new ScrapMetalBuilder instance
        /// </summary>
        public ScrapMetalBuilder()
        {
            _options = new();
        }

        /// <summary>
        /// Sets the auth token ScrapMetalBot will use to authenticate
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ScrapMetalBuilder WithAuth(string token)
        {
            _options._auth = token;
            return this;
        }

        /// <summary>
        /// Sets the size of the buffers the ScrapMetalBot's underlying <see cref="SpeakEasy.SpeakEasySocket"/> will use
        /// </summary>
        /// <param name="sendBufferSize"></param>
        /// <param name="receiveBufferSize"></param>
        /// <returns></returns>
        public ScrapMetalBuilder WithBufferSize(int sendBufferSize, int receiveBufferSize)
        {
            _options._sendBufSize = sendBufferSize;
            _options._recvBufSize = receiveBufferSize;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="CancellationTokenSource"/> used by the ScrapMetalBot. This enables user-level control over the ScrapMetal stack
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <returns></returns>
        public ScrapMetalBuilder WithTokenSource(CancellationTokenSource tokenSource)
        {
            _options._tokenSource = tokenSource;
            return this;
        }

        /// <summary>
        /// Sets the brain used by the ScrapMetalBot. This enables retention of brain details if the ScrapMetalBot instance is recreated
        /// </summary>
        /// <param name="brain"></param>
        /// <returns></returns>
        public ScrapMetalBuilder WithBrain(ScrapMetalBrain brain)
        {
            _options._brain = brain;
            return this;
        }

        /// <summary>
        /// Builds the ScrapMetalBot
        /// </summary>
        /// <returns></returns>
        public ScrapMetalBot Build()
        {
            return new ScrapMetalBot(_options);
        }
    }
}