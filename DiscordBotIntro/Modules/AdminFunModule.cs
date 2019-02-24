using Discord.Commands;
using DiscordBotIntro.Helper;
using DiscordBotIntro.Infrastructure.DI;
using System;
using System.Threading.Tasks;

namespace DiscordBotIntro.Modules
{
    [Name("Fun Admin")]
    [Summary("Have some fun with commands!!!")]
    [Remarks("admin")]
    public class AdminFunModule : ModuleBase<SocketCommandContext>
    {        
        [Command("rolladmin", RunMode = RunMode.Async)]
        [Summary("Roll a dice(1-1000)")]
        [Remarks("*rolladmin*")]
        [Alias("ra")]
        public async Task Roll()
        {
            //check if user is in role in order to proceed with the action
            var adminRole = IResolver.Current.ApplicationSettings.GetBotSettings().DiscordSettings.BotAdminRole;
            var userAllowed = DiscordRoles.UserInRole(Context, adminRole);
            if (!userAllowed)
            {
                var retStr = "\nNot authorized!!!";
                await ReplyAsync($"{retStr}");
                return;
            }

            Random rand1 = new Random();

            int roll = rand1.Next(0, 1000);

            await ReplyAsync($"{this.Context.User.Username} rolled {roll}!!!!!");
        }
    }
}