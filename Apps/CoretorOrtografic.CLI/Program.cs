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
                        Console.WriteLine(Localization.GetNoWordsProvided());
                        PrintInstructions();
                    }
                    else if (readStrings.First().Equals("Q", StringComparison.OrdinalIgnoreCase) || readStrings.First().Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(Localization.GetClosing());
                        Environment.Exit(1);
                    }
                    else if (readStrings.Count == 1)
                    {
                        Console.WriteLine(Localization.GetProvideCommandAndWord());
                        PrintInstructions();
                    }
                    else if (readStrings.First().Length != 1)
                    {
                        Console.WriteLine(Localization.GetUnknownCommand(readStrings.First()));
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
                                Console.WriteLine(Localization.GetUnknownCommand(readStrings.First()));
                                PrintInstructions();
                                break;
                        }
                    }
                }
            }
        }

        private static void PrintInstructions()
        {
            Console.WriteLine(Localization.GetInstructions());
        }

        private static void PrintWordsCorrectness(List<string> words)
        {
            try
            {
                _checker.ExecuteSpellCheck(string.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{processedWord.Original}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($" {Localization.GetIs()} ");
                    if (processedWord.Correct)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Localization.GetCorrect());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(".");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Localization.GetIncorrect());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(".");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception of type {ex.GetType()} occurred.");
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
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{processedWord.Original}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($" {Localization.GetIs()} ");
                    if (processedWord.Correct)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Localization.GetCorrect());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(".");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Localization.GetIncorrect());
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(". ");
                        var suggestedWords = _checker.GetWordSuggestions(processedWord).Result;
                        if (suggestedWords is null || !suggestedWords.Any())
                        {
                            Console.WriteLine(Localization.GetNoSuggestions());
                        }
                        else
                        {
                            Console.Write(Localization.GetSuggestionsAre());
                            foreach (var suggestedWord in suggestedWords)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(suggestedWord);
                                if (suggestedWord != suggestedWords.Last())
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(", ");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(".");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception of type {ex.GetType()} occurred.");
            }
            finally
            {
                Console.WriteLine();
                _checker.CleanSpellChecker();
            }
        }
    }
}
