using System;
namespace Hangman.Modules
{
    public class Menu
    {
        /// <summary>
        /// Initialise and displays the menu
        /// </summary>
        public static void InitialiseMenu()
        {
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Welcome To Hangman!"));

            Console.WriteLine("1: Start the game.\nESC: Exit the game.");

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    Game.Start();
                    break;
                }
                else if(Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("\nYou have chosen the wrong option.");
                }
            }
        }
    }
}
