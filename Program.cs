using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace hangman
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.Write(@"
 _   _                                         
| | | |                                        
| |_| | __ _ _ __   __ _ _ __ ___   __ _ _ __  
|  _  |/ _` | '_ \ / _` | '_ ` _ \ / _` | '_ \ 
| | | | (_| | | | | (_| | | | | | | (_| | | | |
\_| |_/\__,_|_| |_|\__, |_| |_| |_|\__,_|_| |_|
                    __/ |                      
                   |___/    

Made by Ondřej Pešek
-----------------------------------------------
");
      // počet pokusů = počet písmen
      string[] words = File.ReadAllLines(@"word_list.txt");

      for (int i = 0; i < words.Length; i++)
      {
        string word = words[i].ToLower();
        List<char> guessedLetters = new List<char>();

        while (true)
        {
            Console.Clear();
            char input = '\0';
            string output = String.Empty;

            // existují již nějaká uhodnutá písmena?
            if (!guessedLetters.Any())
            {
                output = new String('_', word.Length);
            }
            else
            {
              // print all guessed letters
              Console.WriteLine("guessed letters: "+string.Join(", ", guessedLetters));

              for (int e = 0; e < word.Length; e++)
              {
                if (guessedLetters.Contains(word[e]))
                {
                  output += word[e];
                } else
                {
                  output += "_";
                }
              }

              if (output == word)
              {
                break;
              }
              
              Console.WriteLine(output);
              
            }

            Console.WriteLine(output);

            while (true)
            {
              Console.WriteLine("Vložte písmeno (bez diakritiky):");

              if (!char.TryParse(Console.ReadLine(), out input) && !char.IsLetter(input))
              {
                continue;
              }

              input = char.ToLower(input);

              if (guessedLetters.Contains(input))
              {
                  continue;
              }

              break;
            }

            guessedLetters.Add(input);
        }   
      }
    }
  }
}
