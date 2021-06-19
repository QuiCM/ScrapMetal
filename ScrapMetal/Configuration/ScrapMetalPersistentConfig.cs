namespace ScrapMetal.Configuration
{
    public class ScrapMetalPersistentConfig : PersistentConfiguration
    {
        /// <summary>
        /// Authentication token used to authenticate with Discord's services
        /// </summary>
        public string AuthToken { get; set; }
        /// <summary>
        /// Previously used session id used to attempt reconnections
        /// </summary>
        public string SessionId { get; set; }
    }
}