namespace ScrapMetal.Configuration
{
    public class RuntimeConfiguration
    {
        public PersistableConfigurationItem<string> AuthToken { get; set; }
        public PersistableConfigurationItem<string> SessionId { get; set; }

        public static RuntimeConfiguration LoadFrom(ScrapMetalPersistentConfig configuration)
        {
            return new RuntimeConfiguration
            {
                AuthToken = new PersistableConfigurationItem<string>(configuration, nameof(AuthToken)) { Value = configuration.AuthToken },
                SessionId = new PersistableConfigurationItem<string>(configuration, nameof(SessionId)) { Value = configuration.SessionId }
            };
        }
    }
}