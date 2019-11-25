using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangman.Modules
{
    public class Game
    {
        private static string HangWord;
        private static int IncorrectCount;
        private static int IncorrectThreashhold = 8;
        private static List<char> HangWordLetters = new List<char>();
        private static List<char> GuessedLetters = new List<char>();

        /// <summary>
        /// Starts the game by running the game logic.
        /// </summary>
        public static void Start()
        {
            Console.WriteLine("Game has started.");
            RunGameLogic();
        }

        /// <summary>
        /// Ends the game and redirects to the main menu.
        /// </summary>
        private static void End()
        {
            ResetSettings();
            Console.WriteLine("Game ended. Press any key to go to the main menu...");
            Console.ReadKey();
            Console.Clear();
            Menu.InitialiseMenu();
        }

        /// <summary>
        /// Resets game settings
        /// </summary>
        private static void ResetSettings()
        {
            IncorrectCount = 0;
            HangWord = "";
            HangWordLetters.Clear();
            GuessedLetters.Clear();
        }

        /// <summary>
        /// Runs the game logic
        /// </summary>
        private static void RunGameLogic()
        {
            HangWord = GetRandomWord();
            SaveHangWordLetters(HangWord);

            Console.WriteLine($"Your word has been chosen. It is a {HangWord.Length} lettered word");

            GuessHangWordLetter(Console.ReadKey().KeyChar);
        }

        /// <summary>
        /// Guesses the letters in HangWord.
        /// </summary>
        /// <param name="GuessLetter">Letter to guess</param>
        private static void GuessHangWordLetter(char GuessLetter)
        {
            if (LetterSanityCheck(GuessLetter) == false)
            {
                ResetSettings();
                Console.WriteLine("Congratulations, You have won the game! Press any key to go to the main menu...");
                Console.ReadKey();
                Console.Clear();
                Menu.InitialiseMenu();
            }
        }

        /// <summary>
        /// Callback for GuessHangWordLetter method
        /// </summary>
        private static void GuessHangWordLetterCallBack()
        {
            Console.WriteLine("Have another try of guessing.");

            GuessHangWordLetter(Console.ReadKey().KeyChar);
        }

        /// <summary>
        /// Checks for sanity in GuessLetter
        /// </summary>
        /// <param name="GuessLetter"></param>
        /// <returns>false when guesses are complete. Otherwise, returns true.</returns>
        private static bool LetterSanityCheck(char GuessLetter)
        {
            if (HangWordLetters.Contains(GuessLetter) && !GuessedLetters.Contains(GuessLetter))
            {
                GuessedLetters.Add(GuessLetter);
                Console.WriteLine($"\nCorrect Letter! {GenerateHangmanText()}");

                if (GuessCompleteCheck() == false)
                {
                    GuessHangWordLetterCallBack();
                    return true;
                }
                else { return false; }
            }
            else if (!HangWordLetters.Contains(GuessLetter))
            {
                IncorrectCount++;
                Console.WriteLine($"\nLetter: {GuessLetter} does not exist in the word. {DisplayHangmanStatus()} | Guesses Left: {IncorrectThreashhold - IncorrectCount}");

                if(DisplayHangmanStatus() == "Hanged")
                {
                    DrawHangman();
                    Console.WriteLine($"The hangword was: {HangWord}");
                    End();
                    return true;
                }
                GuessHangWordLetterCallBack();
                return true;
            }
            else
            {
                Console.WriteLine($"\nLetter: {GuessLetter} has already been guessed. Current Guessed Letters: {GetCurrentGuessedLetters()}");

                GuessHangWordLetterCallBack();
                return true;
            }
        }

        /// <summary>
        /// Displays the status of the Hangman
        /// </summary>
        /// <returns>Hangman Status</returns>
        private static string DisplayHangmanStatus()
        {
            if (IncorrectCount == 0) return "Nothing is drawn";
            else if (IncorrectCount == 1) return "Stand has been drawn";
            else if (IncorrectCount == 2) return "Head has been drawn";
            else if (IncorrectCount == 3) return "Body has been drawn";
            else if (IncorrectCount == 4) return "Left arm has been drawn";
            else if (IncorrectCount == 5) return "Right arm has been drawn";
            else if (IncorrectCount == 6) return "Left Leg has been drawn";
            else if (IncorrectCount == 7) return "Right Leg has been drawn";
            else return "Hanged";
        }

        /// <summary>
        /// Draws Hangman Image
        /// </summary>
        private static void DrawHangman()
        {
            foreach(string Line in ReadHangmanImage())
            {
                Console.WriteLine(Line);
            }
        }

        /// <summary>
        /// Checks for guess completion
        /// </summary>
        /// <returns>false if the guesses aren't complete. Otherwise, returns true.</returns>
        private static bool GuessCompleteCheck()
        {
            int FullCount = 0;
            bool CheckFlag;

            foreach(char Letter in HangWord)
            {
                if (GuessedLetters.Contains(Letter)) FullCount++;
            }

            if (FullCount < HangWord.Length)
            {
                CheckFlag = false;
            }
            else
            {
                CheckFlag = true;
            }

            return CheckFlag;
        }

        /// <summary>
        /// Generates hangman text of guesses
        /// </summary>
        /// <returns>hangman text</returns>
        private static string GenerateHangmanText()
        {
            string HangmanText = "";
            StringBuilder sb = new StringBuilder(HangmanText);

            foreach (char Letter in HangWord)
            {
                if (GuessedLetters.Contains(Letter))
                {
                    sb.Append(Letter + " ");
                    HangmanText = sb.ToString();
                }
                else
                {
                    sb.Append("_ ");
                    HangmanText = sb.ToString();
                }
            }

            return HangmanText;
        }

        /// <summary>
        /// Gets current guessed letters
        /// </summary>
        /// <returns>Guessed Letters</returns>
        private static string GetCurrentGuessedLetters()
        {
            string Guessed = "";

            foreach(char Letter in GuessedLetters)
            {
                if(string.IsNullOrEmpty(Guessed))
                {
                    Guessed = Letter.ToString();
                }
                else
                {
                    Guessed = string.Concat(Guessed, ", " + Letter.ToString());
                }
            }

            return Guessed;
        }

        /// <summary>
        /// Saves/Stores the letters in the hangword to HangWordLetters list
        /// </summary>
        /// <param name="HW">Hangword</param>
        private static void SaveHangWordLetters(string HW)
        {
            foreach (char Letter in HW)
            {
                HangWordLetters.Add(Letter);
            }
        }

        /// <summary>
        /// Gets random word from the WordDB
        /// </summary>
        /// <returns>Random word selected</returns>
        private static string GetRandomWord()
        {
            string[] GetWords = ReadWordDB();

            return GetWords[new Random().Next(0, GetWords.Length)];
        }

        /// <summary>
        /// Reads HangmanImage file
        /// </summary>
        /// <returns>HangmanImage</returns>
        private static string[] ReadHangmanImage()
        {
            try
            {
                return File.ReadAllLines(Directory.GetCurrentDirectory() + "//HangmanImage.txt");
            }
            catch (Exception e)
            {
                LogService.Log($"File Read Error Occured\n{e.ToString()}", LogLevel.Error);
                throw new Exception("File Read Error Occured");
            }
        }

        /// <summary>
        /// Reads WordDB
        /// </summary>
        /// <returns>WordDB</returns>
        private static string[] ReadWordDB()
        {
            try
            {
                return File.ReadAllLines(Directory.GetCurrentDirectory() + "//WordDB.txt");
            }
            catch(Exception e)
            {
                LogService.Log($"File Read Error Occured\n{e.ToString()}", LogLevel.Error);
                throw new Exception("File Read Error Occured");
            }
        }
    }
}
