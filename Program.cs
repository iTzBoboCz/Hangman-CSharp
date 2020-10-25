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

      if (!File.Exists(@"word_list.txt"))
      {
        Console.WriteLine("File with words doesn't exist. Creating one!");
        using (StreamWriter writer = File.AppendText(@"word_list.txt"))
        {
          string[] wordsDefault = { "hat", "fish", "recipe", "emotion", "statement", "development" };
          for (int i = 0; i < wordsDefault.Length; i++)
          {
            writer.WriteLine(wordsDefault[i]);
          };
        }
        Console.WriteLine("Done!");
        System.Threading.Thread.Sleep(2000);
      }

      string[] file = File.ReadAllLines(@"word_list.txt");
      List<string> words = new List<string>();

      string inputDifficulty = string.Empty;
      int difficulty;

      while (true)
      {
        do
        {
          Console.Write("Choose your difficulty:\n  [0] => easy\n  [1] => medium\n  [2] => expert\n: ");
          inputDifficulty = Console.ReadLine();
        } while (!int.TryParse(inputDifficulty, out difficulty));

        if (difficulty < 0 || difficulty > 2 )
        {
          continue;   
        }

        break;
      }

      foreach (var word in file)
      {
        if ((word.Length >= 3 && word.Length <= 5) && difficulty == 0) words.Add(word);
        else if ((word.Length >= 6 && word.Length <= 8) && difficulty == 1) words.Add(word);
        else if (word.Length >= 9 && difficulty == 2) words.Add(word);
      }

      Dictionary<char, int> alphabet = new Dictionary<char, int>();

      for (char i = 'a'; i < 'z'; i++)
      {
        alphabet.Add(i, 0);
      }

      // System.Environment.Exit(1);

      // https://developerslogblog.wordpress.com/2020/02/04/how-to-shuffle-an-array/
      Random random = new Random();
      for (int i = words.Count - 1; i > 0; i--)
      {
        int randomIndex = random.Next(0, i + 1);

        string temp = words[i];
        words[i] = words[randomIndex];
        words[randomIndex] = temp;
      }

      int won = 0;
      int lost = 0;

      for (int i = 0; i < words.Count; i++)
      {
        string word = words[i].ToLower();
        
        int lives = words.Count; // počet pokusů = počet písmen
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

            System.Threading.Thread.Sleep(3000);
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
            Console.Write("\n: ");

            // pokud je input validní písmeno
            if (!char.TryParse(Console.ReadLine(), out input) || !char.IsLetter(input))
            {
              Console.WriteLine("You must only use letters.");
              continue;
            }

            // převedení písmena na lowercase
            input = char.ToLower(input);

            // pokud je písmeno v listu možných písmen (všechna bez diakritiky)
            if (!alphabet.ContainsKey(input))
            {
              Console.WriteLine("Only letters without accents are supported.");
              continue;
            }

            // zaznamenání uhádnutí
            alphabet[input]++;

            // pokud hráč písmeno již hádal
            if (guessedLetters.Contains(input))
            {
              Console.WriteLine("You already used this letter.");
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
