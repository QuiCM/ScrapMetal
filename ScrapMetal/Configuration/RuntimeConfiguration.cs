namespace ScrapMetal.Configuration
{
    public class RuntimeConfiguration
    {
        public ConfigurationItem<string> AuthToken { get; set; }
        public ConfigurationItem<string> SessionId { get; set; }

        public static RuntimeConfiguration LoadFrom(ScrapMetalPersistentConfig configuration)
        {
            return new RuntimeConfiguration
            {
                AuthToken = new ConfigurationItem<string>(nameof(AuthToken)) { Value = configuration.AuthToken },
                SessionId = new ConfigurationItem<string>(nameof(SessionId)) { Value = configuration.SessionId }
            };
        }
    }
}