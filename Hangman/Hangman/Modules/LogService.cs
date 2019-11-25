using System;
using System.Reflection;

namespace Hangman.Modules
{
    public class LogService
    {
        /// <summary>
        /// Logs String with Colour
        /// </summary>
        /// <param name="Format"></param>
        /// <param name="FormatColor"></param>
        public static void Log(string Format, LogLevel FormatColor)
        {
            var ConsoleColour = Console.ForegroundColor;

            switch (FormatColor)
            {
                case LogLevel.Debug:
                    ConsoleColour = ConsoleColor.Cyan;
                    break;
                case LogLevel.Error:
                    ConsoleColour = ConsoleColor.Red;
                    break;
                case LogLevel.Warn:
                    ConsoleColour = ConsoleColor.Magenta;
                    break;
                default:
                    // Default color
                    break;
            }

            Console.ForegroundColor = ConsoleColour;

            if (String.IsNullOrEmpty(Format))
            {
                Console.WriteLine($"[{Assembly.GetExecutingAssembly().GetName().Name}] StringNullOrEmpty Occured at ILogService.Log");
                return;
            }

            Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt")} - {Assembly.GetExecutingAssembly().GetName().Name}]: {Format}");
        }
    }
}
