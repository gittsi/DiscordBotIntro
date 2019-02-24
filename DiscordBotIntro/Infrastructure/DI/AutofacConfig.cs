using Autofac;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotIntro.Configuration;

namespace DiscordBotIntro.Infrastructure.DI
{
    public static class AutofacConfig
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<IResolver>().As<IStartable>().SingleInstance();

            //configuration
            builder.RegisterType<SettingsConfiguration>().As<ISettingsConfiguration>().SingleInstance();
            builder.RegisterType<ApplicationSettings>().SingleInstance();           
            
            //discord
            builder.RegisterType<DiscordSocketClient>().SingleInstance();

            //commandService
            builder.RegisterType<CommandService>().InstancePerDependency();
            
            return builder.Build();
        }
    }
}
