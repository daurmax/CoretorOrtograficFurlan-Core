using System;

namespace CoretorOrtografic.CLI
{
    public static class AsciiLogo
    {
        public static void Print()
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
    }
}
