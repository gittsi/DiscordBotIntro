using Discord;
using Discord.Commands;
using DiscordBotIntro.Helper;
using DiscordBotIntro.Infrastructure.DI;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBotIntro.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;        

        public HelpModule(CommandService service)
        {
            _service = service;            
        }

        [Command("info", RunMode = RunMode.Async)]
        [Summary("Get general info")]
        [Remarks("*info*")]
        [Alias("i")]
        public async Task InfoAsync()
        {
            var applicationSettings = IResolver.Current.ApplicationSettings.GetBotSettings();

            string prefix = applicationSettings.DiscordSettings.Prefix;
            Version version = Assembly.GetEntryAssembly().GetName().Version;

            string retStr = "";
            retStr += string.Format("\n{0} - {1}", applicationSettings.GeneralSettings.ApplicationName, applicationSettings.GeneralSettings.Environment);
            retStr += string.Format("\nApplication Version : {0}", version);
            retStr += string.Format("\nPrefix : {0}", prefix);

            await ReplyAsync($"{retStr}");
        }

        [Command("help", RunMode = RunMode.Async)]
        [Summary("Gets general help")]
        [Remarks("*help*")]
        [Alias("h")]
        public async Task HelpAsync()
        {
            var applicationSettings = IResolver.Current.ApplicationSettings.GetBotSettings();
            var prefix = applicationSettings.DiscordSettings.Prefix;

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = $"**{applicationSettings.GeneralSettings.ApplicationName} Commands**",
                Description = $"***Use  {prefix}help <commandname> for details***"
            };

            //foreach (var module in _service.Modules.Where(p => !p.Name.ToLower().Contains("admin")))
            foreach (var module in _service.Modules.Where(p => p.Remarks?.ToLower()!="admin"))
            {
                foreach (var cmd in module.Commands)
                {
                    if (cmd.Aliases.Count > 0 && !cmd.Aliases[0].Contains("help")) //dont give help for help command :p
                    {
                        builder.AddField(x =>
                        {
                            x.Name = string.Join(" | ", cmd.Aliases);
                            x.Value = $"{cmd.Summary}\n";
                            x.IsInline = false;
                        });
                    }
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

        [Command("help-admin", RunMode = RunMode.Async)]
        [Summary("Gets admin help")]
        [Remarks("*help-admin*")]
        [Alias("ha")]
        public async Task HelpAdminAsync()
        {
            var applicationSettings = IResolver.Current.ApplicationSettings.GetBotSettings();
            //check if user is in role in order to proceed with the action
            var adminRole = applicationSettings.DiscordSettings.BotAdminRole;
            var userAllowed = DiscordRoles.UserInRole(Context, adminRole);
            if (!userAllowed)
            {
                var retStr = "\nNot authorized!!!";
                await ReplyAsync($"{retStr}");
                return;
            }

            string prefix = applicationSettings.DiscordSettings.Prefix;

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = $"**{applicationSettings.GeneralSettings.ApplicationName} Commands**",
                Description = $"***Use  {prefix}help <commandname> for details***"
            };

            foreach (var module in _service.Modules.Where(p => p.Remarks?.ToLower() == "admin"))
            {
                foreach (var cmd in module.Commands)
                {
                    if (cmd.Aliases.Count > 0 && !cmd.Aliases[0].Contains("help")) //dont give help for help command :p
                    {
                        builder.AddField(x =>
                        {
                            x.Name = string.Join(" | ", cmd.Aliases);
                            x.Value = $"{cmd.Summary}\n";
                            x.IsInline = false;
                        });
                    }
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

        [Command("help", RunMode = RunMode.Async)]
        [Summary("*Gets helps for specific command*")]
        [Remarks("*help {command}*")]
        public async Task HelpAsync(string command)
        {
            var applicationSettings = IResolver.Current.ApplicationSettings.GetBotSettings();
            //check if user is in role in order to proceed with the action
            var adminRole = applicationSettings.DiscordSettings.BotAdminRole;
            var userAllowed = DiscordRoles.UserInRole(Context, adminRole);

            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            if((!userAllowed && result.Commands.FirstOrDefault().Command.Module.Remarks.ToLower() == "admin"))
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}** that you can check for details.");
                return;
            }

            var prefix = applicationSettings.DiscordSettings.Prefix;
            var builder = new EmbedBuilder();

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                if (cmd.Aliases.Count > 0 && cmd.Aliases[0] != "help" ) //dont give help for help command :p and not admin
                {
                    builder.AddField(x =>
                    {
                        x.Name = string.Join(", ", cmd.Name);
                        x.Value = string.Concat(
                                    cmd.Parameters.Count > 0 ? $"**Parameters**: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" : string.Empty,
                                    $"**Summary**: {cmd.Summary}\n",
                                    $"**Usage**: {prefix}{cmd.Remarks}");
                        x.IsInline = false;
                    });
                }
            }
            await ReplyAsync("", false, builder.Build());
        }
    }
}
