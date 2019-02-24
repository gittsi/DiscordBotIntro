namespace DiscordBotIntro.Configuration
{
    public class SettingsBot
    {
        public GeneralSettings GeneralSettings { get; set; }
        public DiscordSettings DiscordSettings { get; set; }   
    }
    public class GeneralSettings
    {
        public string ApplicationName { get; set; }
        public string Environment { get; set; }
        public string JsonSettingsVersion { get; set; }
    }
    public class DiscordSettings
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public string BotAdminRole { get; set; }
    }  
}
