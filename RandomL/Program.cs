using System.Diagnostics;
using System.Globalization;
using System.Text;

class Program
{
    static void Main()
    {
        Random random = new Random();
        string targetWord = "";

        while (true)
        {
            Console.Write("Please enter the word you want the machine to guess (letters only, no symbols or numbers): ");
            targetWord = Console.ReadLine().ToLower();

            if (IsWordValid(targetWord))
                break;
            else
                Console.WriteLine("Invalid input. Please enter a word containing only letters (no symbols or numbers).");
        }

        string normalizedTargetWord = RemoveDiacritics(targetWord);
        string generatedWord = new string('_', normalizedTargetWord.Length);
        int attempts = 0;
        int wordIndex = 0;
        int correctLetters = 0;

        Stopwatch stopwatch = new Stopwatch();
        Dictionary<char, int> letterFrequencies = new Dictionary<char, int>();

        for (char c = 'a'; c <= 'z'; c++)
        {
            letterFrequencies[c] = 0;
        }

        Console.Clear();
        Console.WriteLine($"Attempts: {attempts}");
        Console.WriteLine($"Current word: {generatedWord}");
        Console.WriteLine("Letters generated:");

        stopwatch.Start();

        while (wordIndex < normalizedTargetWord.Length)
        {
            char randomLetter = (char)random.Next('a', 'z' + 1);
            attempts++;

            letterFrequencies[randomLetter]++;

            if (randomLetter == normalizedTargetWord[wordIndex])
            {
                generatedWord = generatedWord.Remove(wordIndex, 1).Insert(wordIndex, randomLetter.ToString());
                correctLetters++;
                wordIndex++;
            }

            Console.SetCursorPosition(10, 0);
            Console.Write(attempts);

            Console.SetCursorPosition(14, 1);
            Console.Write(generatedWord);

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Letters generated:");

            Console.SetCursorPosition(0, 3);
            for (char c = 'a'; c <= 'z'; c++)
            {
                Console.Write($"{c}: {letterFrequencies[c],2}  ");
                if ((c - 'a' + 1) % 5 == 0) Console.WriteLine();
            }

            double progress = ((double)correctLetters / normalizedTargetWord.Length) * 100;
            Console.SetCursorPosition(0, Math.Min(Console.BufferHeight - 3, 24));
            Console.WriteLine($"Progress: {progress:F2}%");

            double successRate = ((double)correctLetters / attempts) * 100;
            Console.WriteLine($"Success rate: {successRate:F2}%");

            Thread.Sleep(50);
        }

        stopwatch.Stop();

        Console.SetCursorPosition(0, Math.Min(Console.BufferHeight - 2, 26));
        Console.WriteLine($"\nSuccess! Matched the word '{targetWord}' in {attempts} attempts.");
        Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");
    }

    static bool IsWordValid(string word)
    {
        foreach (char c in word)
        {
            if (!char.IsLetter(c))
                return false;
        }
        return true;
    }

    static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
