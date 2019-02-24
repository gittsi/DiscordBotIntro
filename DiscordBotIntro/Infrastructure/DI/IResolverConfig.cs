using Autofac;
using DiscordBotIntro.Configuration;

namespace DiscordBotIntro.Infrastructure.DI
{
    public abstract class ResolverConfig
    {
        internal IContainer Container { get; set; }
        public ApplicationSettings ApplicationSettings { get { return Container.Resolve<ApplicationSettings>(); } }
        public SettingsBot SettingsBot { get { return Container.Resolve<SettingsBot>(); } }
        
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
        
            builder.RegisterType<ApplicationSettings>().SingleInstance();
            builder.RegisterType<SettingsBot>().SingleInstance();
            builder.RegisterType<SettingsConfiguration>().As<ISettingsConfiguration>().SingleInstance();

            return builder.Build();
        }
    }
}
