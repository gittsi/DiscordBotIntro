using Microsoft.Extensions.Configuration;

namespace DiscordBotIntro.Configuration
{
    public interface ISettingsConfiguration
    {
        IConfigurationRoot GetConfiguration();
    }
}
