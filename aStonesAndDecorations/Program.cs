namespace Yandex.Cup2020.Qual.Train.A.StonesAndDecorations
{
    using System;
    using System.IO;

    class Program
    {
        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";

        static void Main(string[] args)
        {
            var input = ReadInput().Split(new[]{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            if (input.Length < 2)
            {
                WriteOutput(0);
                return;
            }

            var stones = input[1];
            var decorations = input[0];

            int decorationsCount = 0;
            foreach (var stone in stones)
            {
                if (decorations.Contains(stone.ToString()))
                {
                    decorationsCount++;
                }
            }

            WriteOutput(decorationsCount);
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