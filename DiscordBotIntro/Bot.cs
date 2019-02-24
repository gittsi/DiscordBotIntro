using Autofac;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotIntro.Modules;
using DiscordBotIntro.Configuration;
using DiscordBotIntro.Helper;
using DiscordBotIntro.Infrastructure.DI;
using System;
using System.Threading.Tasks;

namespace DiscordBotIntro
{
    public class Bot
    {
        static void Main(string[] args) => new Bot().MainAsync().GetAwaiter().GetResult();

        private IContainer autoFacContainer = null;
        private ApplicationSettings applicationSettings = null;
        private DiscordSocketClient client = null;
        private CommandService commands = null;
        private SettingsBot settingsBot = null;

        public async Task MainAsync()
        {
            ///////////initialize autofac
            autoFacContainer = AutofacConfig.ConfigureContainer();
            using (var scope = autoFacContainer.BeginLifetimeScope())
            {
                applicationSettings = scope.Resolve<ApplicationSettings>();
                commands = scope.Resolve<CommandService>();
                client = scope.Resolve<DiscordSocketClient>();

                settingsBot = applicationSettings.GetBotSettings();

                await InstallCommands();
                await client.LoginAsync(TokenType.Bot, settingsBot.DiscordSettings.Token);
                await client.StartAsync();
                await client.SetGameAsync(string.Format("{0}help", settingsBot.DiscordSettings.Prefix));
            }

            while (client.ConnectionState != ConnectionState.Connected)
            {
                Consoler.WriteLineInColor(string.Format("Still not connected... {0}", DateTime.Now), ConsoleColor.Yellow);
                await Task.Delay(2000);
            }
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task InstallCommands()
        {
            client.Log += Log;
            client.MessageReceived += HandleCommandAsync;

            //add modules
            await commands.AddModuleAsync<AdminFunModule>(null);
            await commands.AddModuleAsync<FunModule>(null);
            await commands.AddModuleAsync<HelpModule>(null);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            var msg = arg as SocketUserMessage;
            if (msg == null)
            {
                return;
            }

            // We don't want the bot to respond to itself or other bots.
            // NOTE: Selfbots should invert this first check and remove the second
            // as they should ONLY be allowed to respond to messages from the same account.
            if (msg.Author.Id == client.CurrentUser.Id || msg.Author.IsBot) return;

            // Create a number to track where the prefix ends and the command begins
            int pos = 0;
            
            var prefix = applicationSettings.GetBotSettings().DiscordSettings.Prefix;
            if (msg.HasCharPrefix(Convert.ToChar(prefix), ref pos) || msg.HasMentionPrefix(client.CurrentUser, ref pos))
            {
                // Create a Command Context.
                var context = new SocketCommandContext(client, msg);

                Consoler.WriteLineInColor(string.Format("User : '{0}' sent the following command : '{1}'", context.Message.Author.ToString(), context.Message.ToString()), ConsoleColor.Green);
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed succesfully).
                var result = await commands.ExecuteAsync(context, pos, null);

                // Comment the following lines if you don't want the bot
                // to send a message if it failed (advised for most situations).
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Consoler.WriteLineInColor(string.Format("error  : '{0}' ", result.ErrorReason), ConsoleColor.Green);
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
                }

                if (result.Error == CommandError.UnknownCommand)
                {
                    var message = msg.Channel.SendMessageAsync($"I am pretty sure that there is no command `{msg}`!!!\nTry `{prefix}help` to get an idea!").Result;
                    await Task.Delay(3000);
                    await message.DeleteAsync();
                }
            }
        }
    }
}
