using CoretorOrtografic.Infrastructure.SpellChecker;
using CoretorOrtografic.Core.Input;
using CoretorOrtografic.Core.SpellChecker;
using Autofac;
using System;
using System.Threading;
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

            PrintTitle();
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
                        Console.WriteLine("No commands or words were provided.");
                        PrintInstructions();
                    }
                    else if (readStrings.First().ToUpper() == "Q" || readStrings.First().ToUpper() == "QUIT")
                    {
                        Console.WriteLine("Closing application...");
                        Environment.Exit(1);
                    }
                    else if (readStrings.Count == 1)
                    {
                        Console.WriteLine("Please provide a command and at least a word to check.");
                        PrintInstructions();
                    }
                    else if (readStrings.First().Length != 1)
                    {
                        Console.WriteLine($"Unknown command '{readStrings.First()}'.");
                        PrintInstructions();
                    }
                    else
                    {
                        switch (Char.ToUpper(readStrings.First().Single()))
                        {
                            case 'C':
                                PrintWordsCorrectness(readStrings.Skip(1).ToList());
                                break;
                            case 'S':
                                PrintSuggestedWords(readStrings.Skip(1).ToList());
                                break;
                            default:
                                Console.WriteLine($"Unknown command '{readStrings.First()}'.");
                                PrintInstructions();
                                break;
                        }
                    }
                }
            }
        }

        public static void PrintTitle()
        {
            Console.Title = "ASCII Art";
            string title = @"
                                                                                      
                                                ******************                                   
                                            ************************+++                             
                                          *************************+++++++++*                       
                                        *************************++++++++++++++++       +=+=+       
                                ===~~~~+***********************+++++++++++++++++++++++++++==+       
                           +=======~~~~=+********************+++++++++++++++++++++++++++=====       
                        +++========~~~~~+******************+++++++++++++++++++++++++++======        
                      ++++++=======~~~~~=****************++++++++++++++++++++++++++++=======        
                    ++++++++=======~~~~~~+**************+++++++++++++++++++++++++++========         
                  +*++++++++=======~~~~~~=*************+++++++++++++++++++++++++++========          
                  **++++++++=======~~~~~~~+**********+++++++++++++++++++++++++++========            
                 ***++++++++=======~~~~~~~~+*******+++++++++++++++++++++++++++========              
                 ***++++++++=======~~~~~~~~-+****+++++++++++++++++++++++++++==========              
       >>><      ***++++++++=======~~~~~~~~--=++++++++++++++++++++++++++++============              
        >><<<<<             (((((()<>+~~~~~---~+++++++++++++++++++++++++==============              
         ><<<<<<<<<<<))))))))((((((((((<>=~-----=++++++++++++++++++++++================             
          <<<<<<<<<<)))))))))((((((((((]]](>=----~=++++++++++++++++++==================             
           <<<<<<<<<))))))))))<>>^^*+==~~~~~-------~=++++++++++++++=====================            
              <<<<<<)<<>>^++=======~~~~~~~~-------:::-=++++++++++=======================            
                      ++++++=======~~~~~~~~-----~~=++++++++++++=========================            
                         ++========~~~~~~~~--~+++++++++++++++==========================~=           
                              +====~~~~~      +++++++++++++++++======================~~~~           
                                              +=+++++++++++++++++==================~~~~~~           
                                              ==+++++++++++++++++++===============~~~~==+^          
                                              ====+++++++++++++++++++===========~~==*>))<<          
                                              ======++++++++++++++++++=======~==*<)))<<<<<          
                                              ========++++++++++++++++++=====>))))<<<<<<<<>         
                                              ==========++++++++++++++++**<)))<<<<<<<<<<>>>         
                                              ===========+++++++++++++^<)))<<<<<<<<<<>>>>>>         
                                              =============++++++++*><<<<<<<<<<<<>>>>>>>>>>         
                                             ================++++^<<<<<<<<<<<<>>>>>>>>>             
                                             =~==============+*><<<<<<<<<<>>>>>>>>>>                
                                             ~~~~==========+^<<<<<<<<<<>>>>>>>>>>                   
                                             ~~~~~=======+><<<<<<<<>>>>>>>>>>>                      
                                            ~~~~~~~~===*><<<<<<<>>>>>>>>>>>                         
                                            ~~~~~~~~=*><<<<<>>>>>>>>>>>>                            
                                           ~~~~~~~=*><<<>>>>>>>>>>>>>                               
                  ]]]]                    -~~~~~=*><<>>>>>>>>>>>>>>                                 
               ]]]]]]]]                  ~-~~~=*>>>>>>>>>>>>>>>>                                    
             ]]]]]]]](((                ----~*>>>>>>>>>>>>>>^>                                      
             (]]]((((((((               --~*>>>>>>>>>>>>>>^                                         
              (((((()))))))           ---+>>>>>>>>>>>^^^>                                           
               (())))))))<<<         --=^>>>>>>>>^^^^^>                                             
               )))))))<<<<<<<>      -=^>>>>>>^^^^^^^^                                               
                ))<<<<<<<<<<>>><   =*>>>>>^^^^^^^^^                                                 
                 <<<<<<<>>>>>>>>>^^>>>^^^^^^^^^^^                                                   
                  <<<>>>>>>>>>>>>>>^^^^^^^^^^^^                                                     
                    >>>>>>>>>>>^^^^^^^^^^^^^^                                                       
                     >>>>>>>^^^^^^^^^^^^^^^                                                         
                      >>^^^^^^^^^^^^^^^^^^                                                          
                        ^^^^^^^^^^^^^^^^                                                            
                         ^^^^^^^^^^^^^^                                                             
                           ^^^^^^^^^^                                                               
                            ^^^^^^^                                                                 
                              ^^^^                                                                                  
                                                                                                    
        ";

            Console.WriteLine(title);
        }
        public static void PrintInstructions()
        {

        }
        private static void PrintWordsCorrectness(List<string> words)
        {
            try
            {
                _checker.ExecuteSpellCheck(String.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{processedWord.Original}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" is ");
                    if (processedWord.Correct)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("correct");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(".");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("incorrect");
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
                _checker.ExecuteSpellCheck(String.Join(" ", words));

                foreach (ProcessedWord processedWord in _checker.ProcessedWords)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{processedWord.Original}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" is ");
                    if (processedWord.Correct)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("correct");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(".");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("incorrect");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(". ");
                        var suggestedWords = _checker.GetWordSuggestions(processedWord).Result;
                        if (suggestedWords is null || !suggestedWords.Any())
                        {
                            Console.WriteLine("There are no suggestions.");
                        }
                        else
                        {
                            Console.Write("Suggestions are: ");
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
