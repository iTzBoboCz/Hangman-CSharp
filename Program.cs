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
      string[] words = File.ReadAllLines(@"word_list.txt");

      // https://developerslogblog.wordpress.com/2020/02/04/how-to-shuffle-an-array/
      Random random = new Random();
      for (int i = words.Length - 1; i > 0; i--)
      {
        int randomIndex = random.Next(0, i + 1);

        string temp = words[i];
        words[i] = words[randomIndex];
        words[randomIndex] = temp;
      }

      int won = 0;
      int lost = 0;

      for (int i = 0; i < words.Length; i++)
      {
        string word = words[i].ToLower();
        
        int lives = words.Length; // počet pokusů = počet písmen
        List<char> guessedLetters = new List<char>();

        while (true)
        {
            if (lives < 1)
            {
              Console.Clear();

              lost++;

              Console.ForegroundColor = ConsoleColor.Red;
              Console.WriteLine("You just lost!\nMaybe you'll win the next one!");
              Console.ResetColor();

              System.Threading.Thread.Sleep(1000);
              break;
            }

            Console.Clear();
            char input = '\0';
            string output = String.Empty;
            
            Console.WriteLine($"{won} won | {lost} lost");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new String((char)3, lives)); // (char)3 = ♥
            Console.ResetColor();

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
                won++;
                break;
              }              
            }

            Console.WriteLine(output);

            // hádání písmena
            while (true)
            {
              Console.WriteLine(":");

              // pokud je input validní písmeno
              if (!char.TryParse(Console.ReadLine(), out input) && !char.IsLetter(input))
              {
                continue;
              }

              // převedení písmena na lowercase
              input = char.ToLower(input);

              // pokud hráč písmeno již nehádal
              if (guessedLetters.Contains(input))
              {
                  continue;
              }

              // pokud hádané písmeno ve slově není, odebere 1 život
              if (!word.Contains(input))
              {
                  lives--;
              }
              break;
            }

            // pokud se splní podmínky, písmeno se přidá do listu uhádnutých písmen
            guessedLetters.Add(input);
        }   
      }
      Console.WriteLine("There are no more words!\n\nThank you for playing. :))");
    }
  }
}
