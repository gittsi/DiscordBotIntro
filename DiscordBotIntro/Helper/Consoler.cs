using System;

namespace DiscordBotIntro.Helper
{
    public static class Consoler
    {
        public static void WriteLineInColor(string msg, ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ResetColor();
            }
            catch (Exception) { }
        }
    }
}
