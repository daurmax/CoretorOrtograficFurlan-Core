using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CoretorOrtografic.CLI
{
    public enum Language
    {
        Furlan,
        Italian,
        English
    }

    public class LocalizationStrings
    {
        public string Instructions { get; set; }
        public string NoWordsProvided { get; set; }
        public string ProvideCommandAndWord { get; set; }
        public string Closing { get; set; }
        public string Is { get; set; }
        public string Correct { get; set; }
        public string Incorrect { get; set; }
        public string SuggestionsAre { get; set; }
        public string NoSuggestions { get; set; }
        public string UnknownCommandFormat { get; set; }
    }

    public static class Localization
    {
        private static readonly Dictionary<Language, LocalizationStrings> Strings;

        static Localization()
        {
            Strings = new Dictionary<Language, LocalizationStrings>
            {
                { Language.English, LoadStrings("en.json") },
                { Language.Italian, LoadStrings("it.json") },
                { Language.Furlan,  LoadStrings("fur.json") }
            };
        }

        public static Language Current { get; private set; } = Language.English;

        public static void SelectLanguage()
        {
            Console.WriteLine("Select language: [F]urlan, [I]taliano, [E]nglish");
            while (true)
            {
                var key = Console.ReadKey(true).KeyChar;
                switch (char.ToUpperInvariant(key))
                {
                    case 'F':
                        Current = Language.Furlan;
                        return;
                    case 'I':
                        Current = Language.Italian;
                        return;
                    case 'E':
                        Current = Language.English;
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Use F, I or E.");
                        break;
                }
            }
        }

        private static LocalizationStrings LoadStrings(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Localization", fileName);
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<LocalizationStrings>(json);
        }

        public static string GetInstructions() => Strings[Current].Instructions;
        public static string GetNoWordsProvided() => Strings[Current].NoWordsProvided;
        public static string GetProvideCommandAndWord() => Strings[Current].ProvideCommandAndWord;
        public static string GetClosing() => Strings[Current].Closing;
        public static string GetIs() => Strings[Current].Is;
        public static string GetCorrect() => Strings[Current].Correct;
        public static string GetIncorrect() => Strings[Current].Incorrect;
        public static string GetSuggestionsAre() => Strings[Current].SuggestionsAre;
        public static string GetNoSuggestions() => Strings[Current].NoSuggestions;
        public static string GetUnknownCommand(string cmd) =>
            string.Format(Strings[Current].UnknownCommandFormat, cmd);
    }
}
