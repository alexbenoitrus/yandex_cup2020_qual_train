namespace Yandex.Cup2020.Qual.Train.F.fSummOfNumbers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;

    class Program
    {
        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";

        private static string Host;
        private static string Port;
        private static string A;
        private static string B;

        static void Main(string[] args)
        {
            GetInput();
            WriteOutput(Process());
        }

        private static int Process()
        {
            var request = (HttpWebRequest)WebRequest.Create($"{Host}:{Port}?a={A}&b={B}");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var json = streamReader.ReadToEnd();
                var numbers = JsonConvert.DeserializeObject<List<int>>(json);

                return numbers.Sum();
            }
        }

        private static void GetInput()
        {
            var input = ReadInput().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Host = input[0];
            Port = input[1];
            A = input[2];
            B = input[3];
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
