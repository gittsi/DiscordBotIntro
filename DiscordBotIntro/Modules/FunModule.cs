using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBotIntro.Modules
{
    [Name("Fun")]
    [Summary("Have some fun with commands!!!")]
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        [Command("roll", RunMode = RunMode.Async)]
        [Summary("Roll a dice(1-100)")]
        [Remarks("*roll*")]
        [Alias("r")]
        public async Task Roll()
        {
            Random rand1 = new Random();

            int roll = rand1.Next(0, 100);

            await ReplyAsync($"{this.Context.User.Username} rolled {roll}!!!!!");
        }
    }
}