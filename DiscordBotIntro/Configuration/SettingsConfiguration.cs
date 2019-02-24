using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DiscordBotIntro.Configuration
{
    public class SettingsConfiguration : ISettingsConfiguration
    {
        public static IConfigurationRoot Configuration { get; set; }
        public SettingsConfiguration()
        {
            Configuration = GetConfiguration();
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(string.Concat(Directory.GetCurrentDirectory(), "/JsonAppSettings"))
            .AddJsonFile("configBot.json", optional: false, reloadOnChange: true);//bot settings       

            Console.WriteLine("Read Configuration file");

            Configuration = builder.Build();

            return Configuration;
        }
    }
}
