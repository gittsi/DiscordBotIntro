using Microsoft.Extensions.Configuration;

namespace DiscordBotIntro.Configuration
{
    public class ApplicationSettings : IApplicationSettings
    {
        private readonly IConfigurationRoot _SettingsConfigurationRoot;
        public ApplicationSettings(ISettingsConfiguration settingsConfiguration)
        {
            _SettingsConfigurationRoot = settingsConfiguration.GetConfiguration();
        }
        public SettingsBot GetBotSettings()
        {
            SettingsBot appSettings = new SettingsBot
            {
                //general settings
                GeneralSettings = new GeneralSettings()
                {
                    ApplicationName = _SettingsConfigurationRoot.GetSection("General_Settings")["ApplicationName"]
                    ,
                    Environment = _SettingsConfigurationRoot.GetSection("General_Settings")["Environment"]
                    ,
                    JsonSettingsVersion = _SettingsConfigurationRoot.GetSection("General_Settings")["JsonSettingsVersion"]                    
                },
                //discord settings
                DiscordSettings = new DiscordSettings()
                {
                    Token = _SettingsConfigurationRoot.GetSection("Discord_Settings")["Token"]
                     ,
                    Prefix = _SettingsConfigurationRoot.GetSection("Discord_Settings")["Prefix"]
                     ,
                    BotAdminRole = _SettingsConfigurationRoot.GetSection("Discord_Settings")["BotAdminRole"]

                }
            };
            return appSettings;
        }      
    }
}
