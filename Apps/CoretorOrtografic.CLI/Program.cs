using CoretorOrtografic.Infrastructure.SpellChecker;
using CoretorOrtografic.Core.Input;
using CoretorOrtografic.Core.SpellChecker;
using Autofac;
using System;
using System.Linq;
using System.Collections.Generic;
using Components.CoretorOrtografic.Entities.ProcessedElements;

namespace CoretorOrtografic.CLI
{
    public class Program
    {
        private static IContainer _container;
        private static IContentReader _reader;
        private static ISpellChecker _checker;

        private static void WriteColored(string text, ConsoleColor color, bool newLine = true)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
            Console.ForegroundColor = prev;
        }

        public static void Main(string[] args)
        {
            _container = CoretorOrtograficCliDependencyContainer.Configure
#if DEBUG
                (true);
#else
                (false);
#endif
            Console.ForegroundColor = ConsoleColor.White;

            Localization.SelectLanguage();
            AsciiLogo.Print();
            PrintInstructions();

            using (var scope = _container.BeginLifetimeScope())
            {
                _reader = scope.Resolve<IContentReader>();
                _checker = scope.Resolve<ISpellChecker>();

                while (true)
                {
                    var readStrings = _reader.Read()?.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (readStrings is null || !readStrings.Any())
                    {
                        WriteColored(Localization.GetNoWordsProvided(), ConsoleColor.Yellow);
                        PrintInstructions();
                    }
                    else if (readStrings.First().Equals("Q", StringComparison.OrdinalIgnoreCase) || readStrings.First().Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                    {
                        WriteColored(Localization.GetClosing(), ConsoleColor.Green);
                        Environment.Exit(1);
                    }
                    else if (readStrings.Count == 1)
                    {
                        WriteColored(Localization.GetProvideCommandAndWord(), ConsoleColor.Yellow);
                        PrintInstructions();
                    }
                    else if (readStrings.First().Length != 1)
                    {
                        WriteColored(Localization.GetUnknownCommand(readStrings.First()), ConsoleColor.Red);
                        PrintInstructions();
                    }
                    else
                    {
                        switch (char.ToUpperInvariant(readStrings.First().Single()))
                        {
                            case 'C':
                                PrintWordsCorrectness(readStrings.Skip(1).ToList());
                                break;
                            case 'S':
                                PrintSuggestedWords(readStrings.Skip(1).ToList());
                                break;
                            default:
                                WriteColored(Localization.GetUnknownCommand(readStrings.First()), ConsoleColor.Red);
                                PrintInstructions();
                                break;
                        }
                    }
                }
            }
        }

        private static void PrintInstructions()
        {
            Console.WriteLine();
            WriteColored("=============================================", ConsoleColor.DarkCyan);
            WriteColored(Localization.GetInstructions(), ConsoleColor.Cyan);
            WriteColored("=============================================", ConsoleColor.DarkCyan);
            Console.WriteLine();

            void PrintCommand(char cmd, string description)
            {
                WriteColored($" {cmd}", ConsoleColor.Yellow, false);
                WriteColored($"  - {description}", ConsoleColor.White);
            }

            PrintCommand('C', Localization.GetCorrect());
            PrintCommand('S', Localization.GetSuggestionsAre());
            PrintCommand('Q', Localization.GetClosing());

            Console.WriteLine();
        }

        private static void PrintWordsCorrectness(List<string> words)
        {
            try
            {
                _checker.ExecuteSpellCheck(string.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
                {
                    WriteColored(processedWord.Original, ConsoleColor.Blue, false);
                    WriteColored($" {Localization.GetIs()} ", ConsoleColor.White, false);
                    if (processedWord.Correct)
                    {
                        WriteColored(Localization.GetCorrect(), ConsoleColor.Green, false);
                        WriteColored(".", ConsoleColor.White);
                    }
                    else
                    {
                        WriteColored(Localization.GetIncorrect(), ConsoleColor.Red, false);
                        WriteColored(".", ConsoleColor.White);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteColored($"An exception of type {ex.GetType()} occurred.", ConsoleColor.Red);
            }
            finally
            {
                Console.WriteLine();
                _checker.CleanSpellChecker();
            }
        }

        private static void PrintSuggestedWords(List<string> words)
        {
            try
            {
                _checker.ExecuteSpellCheck(string.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
                {
                    WriteColored(processedWord.Original, ConsoleColor.Blue, false);
                    WriteColored($" {Localization.GetIs()} ", ConsoleColor.White, false);
                    if (processedWord.Correct)
                    {
                        WriteColored(Localization.GetCorrect(), ConsoleColor.Green, false);
                        WriteColored(".", ConsoleColor.White);
                    }
                    else
                    {
                        WriteColored(Localization.GetIncorrect(), ConsoleColor.Red, false);
                        WriteColored(". ", ConsoleColor.White, false);
                        var suggestedWords = _checker.GetWordSuggestions(processedWord).Result;
                        if (suggestedWords is null || !suggestedWords.Any())
                        {
                            WriteColored(Localization.GetNoSuggestions(), ConsoleColor.Yellow);
                        }
                        else
                        {
                            WriteColored(Localization.GetSuggestionsAre(), ConsoleColor.White, false);
                            foreach (var suggestedWord in suggestedWords)
                            {
                                WriteColored(suggestedWord, ConsoleColor.Yellow, false);
                                if (suggestedWord != suggestedWords.Last())
                                {
                                    WriteColored(", ", ConsoleColor.White, false);
                                }
                                else
                                {
                                    WriteColored(".", ConsoleColor.White);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteColored($"An exception of type {ex.GetType()} occurred.", ConsoleColor.Red);
            }
            finally
            {
                Console.WriteLine();
                _checker.CleanSpellChecker();
            }
        }
    }
}
