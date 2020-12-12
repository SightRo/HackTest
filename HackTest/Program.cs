using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HackTest
{
    static class Program
    {
        static readonly Regex AnswerRegex = new Regex(@"([0|1]†){2,}");

        static void Main(string[] args)
        {
            Console.WriteLine("Укажите путь к тесту:");
            var path = Console.ReadLine();
            
            while (true)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("Указанного файла не существует. Попробуйте указать путь еще раз:");
                    path = Console.ReadLine();
                    continue;
                }

                break;
            }
            

            var testExeFile = new FileInfo(path);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = testExeFile.FullName
                }
            };
            process.Start();
            process.WaitForExit();

            var directory = testExeFile.Directory.GetDirectories()
                    .FirstOrDefault(d => string.CompareOrdinal(d.Name, @"test_cheburashka_rulizZz_") == 0);

            var testFile = directory.GetFiles()
                .FirstOrDefault(file => string.CompareOrdinal(file.Extension, ".tbp") == 0);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var testData = File.ReadAllText(testFile.FullName, Encoding.GetEncoding(1251));
            var matches = AnswerRegex.Matches(testData);

            for (int i = 0; i < matches.Count; i++)
                Console.WriteLine($"{i + 1}) {GetCorrectAnswer(matches[i].Value)}");

            Console.ReadKey();
        }

        static int GetCorrectAnswer(string group)
        {
            int numberCount = 1;
            foreach (var letter in group)
            {
                if (char.IsDigit(letter))
                {
                    if (letter == '1')
                        return numberCount;
                    numberCount++;
                }
            }

            return -1;
        }
    }
}