namespace Yandex.Cup2020.Qual.Train.C.InterestingGame
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        private class Player
        {
            public Player(string name)
            {
                this.Name = name;
            }

            public string Name { get; }

            public int Count { get; private set; }

            public void IncrementCount()
            {
                this.Count++;
            }
        }

        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";

        private static int WinCount;

        private static IEnumerable<int> Cards;

        static void Main(string[] args)
        {
            GetInput();
            WriteOutput(Process());
        }

        private static string Process()
        {
            var vasya = new Player("Vasya");
            var petya = new Player("Petya");

            foreach (var card in Cards)
            {
                if (petya.Count == WinCount) return petya.Name;
                if (vasya.Count == WinCount) return vasya.Name;

                bool isFive = card % 5 == 0;
                bool isThree = card % 3 == 0;

                if (isFive && isThree) continue;
                if (isFive) vasya.IncrementCount();
                if (isThree) petya.IncrementCount();
            }

            if (petya.Count == vasya.Count) return "Draw";

            return petya.Count > vasya.Count 
                ? petya.Name
                : vasya.Name;
        }

        private static void GetInput()
        {
            var input = ReadInput().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            WinCount = int.Parse(input[0].Split(' ')[0]);
            Cards = input[1].Split(' ').Select(int.Parse);
        }

        private static string ReadInput()
        {
            using (var file = File.Open(InputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }

        private static void WriteOutput(object value)
        {
            WriteOutput(value.ToString());
        }

        private static void WriteOutput(string value)
        {
            using (var writer = new StreamWriter(OutputFile, true))
            {
                writer.Write(value);
            }
        }
    }
}