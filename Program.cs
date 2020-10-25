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

      string inputGameStyle = String.Empty;
      int gameStyle;
      List<string> words = new List<string>();

      // Game style: words or sentences
      while (true)
      {
        do
        {
          Console.Write("Choose game style:\n  [0] => words\n  [1] => sentences\n: ");
          inputGameStyle = Console.ReadLine();
        } while (!int.TryParse(inputGameStyle, out gameStyle));

        if (gameStyle < 0 || gameStyle > 1)
        {
          continue;
        }

        break;
      }

      if (gameStyle == 0)
      {
        // if file doesn't exist
        if (!File.Exists(@"word_list.txt"))
        {
          Console.WriteLine("File with words doesn't exist. Creating one!");
          using (StreamWriter writer = File.AppendText(@"word_list.txt"))
          {
            string[] wordsDefault = { "hat", "fish", "recipe", "emotion", "statement", "development" };
            for (int i = 0; i < wordsDefault.Length; i++)
            {
              // write default words
              writer.WriteLine(wordsDefault[i]);
            };
          }
          Console.WriteLine("Done!");
          System.Threading.Thread.Sleep(2000);
        }

        string[] file = File.ReadAllLines(@"word_list.txt");

        string inputDifficulty = string.Empty;
        int difficulty;

        // Difficulty: easy, medium or expert
        while (true)
        {
          do
          {
            Console.Write("Choose your difficulty:\n  [0] => easy\n  [1] => medium\n  [2] => expert\n: ");
            inputDifficulty = Console.ReadLine();
          } while (!int.TryParse(inputDifficulty, out difficulty));

          if (difficulty < 0 || difficulty > 2)
          {
            continue;
          }

          break;
        }

        // Use words according to selected difficulty
        foreach (var word in file)
        {
          if ((word.Length >= 3 && word.Length <= 5) && difficulty == 0) words.Add(word);
          else if ((word.Length >= 6 && word.Length <= 8) && difficulty == 1) words.Add(word);
          else if (word.Length >= 9 && difficulty == 2) words.Add(word);
        }
      } else {
        // if file doesn't exist
        if (!File.Exists(@"sentence_list.txt"))
        {
          Console.WriteLine("File with sentences doesn't exist. Creating one!");
          using (StreamWriter writer = File.AppendText(@"sentence_list.txt"))
          {
            string[] wordsDefault = {
              "Sometimes I stare at a door or a wall and I wonder what is this reality, why am I alive, and what is this all about?",
              "He picked up trash in his spare time to dump in his neighbor's yard.",
              "The beach was crowded with snow leopards.",
              "The fifty mannequin heads floating in the pool kind of freaked them out.",
              "If you don't like toenails, you probably shouldn't look at your feet.",
              "She was too short to see over the fence."
            };
            for (int i = 0; i < wordsDefault.Length; i++)
            {
              // write default sentences
              writer.WriteLine(wordsDefault[i]);
            };
          }
          Console.WriteLine("Done!");
          System.Threading.Thread.Sleep(2000);
        }

        string[] file = File.ReadAllLines(@"sentence_list.txt");

        // Array to list
        foreach (var word in file)
        {
          words.Add(word);
        }
      }

      // List of allowed letters
      Dictionary<char, int> alphabet = new Dictionary<char, int>();

      for (char i = 'a'; i <= 'z'; i++)
      {
        alphabet.Add(i, 0);
      }

      // randomise words/sentences (https://developerslogblog.wordpress.com/2020/02/04/how-to-shuffle-an-array/)
      Random random = new Random();
      for (int i = words.Count - 1; i > 0; i--)
      {
        int randomIndex = random.Next(0, i + 1);

        string temp = words[i];
        words[i] = words[randomIndex];
        words[randomIndex] = temp;
      }

      // stats
      int won = 0;
      int lost = 0;

      for (int i = 0; i < words.Count; i++)
      {
        string word = words[i].ToLower();
        
        int lives = (words[i].Length > alphabet.Count / 3) ? (alphabet.Count / 3) : words[i].Length; // počet pokusů = počet písmen
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

          // print all guessed letters
          Console.WriteLine("guessed letters: "+string.Join(", ", guessedLetters));

          for (int e = 0; e < word.Length; e++)
          {
            if (guessedLetters.Contains(word[e]))
            {
              output += word[e];
            } else if (!char.IsLetter(word[e]))
            {
              output += word[e];
            }
            else
            {
              output += "_";
            }
          }

          if (output == word)
          {
            won++;
            break;
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

      string gameStyleWord;
      gameStyleWord = (gameStyle == 0) ? "word" : "sentence";

      Console.WriteLine($"There are no more {gameStyleWord}s!\n\nThank you for playing. :))");
    }
  }
}
