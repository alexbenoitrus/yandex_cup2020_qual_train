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
        private static string A = "0";
        private static string B = "0";

        static void Main(string[] args)
        {
            GetInput();
            WriteOutput(Process());
        }

        private static long Process()
        {
            var uri = $"{Host}:{Port}?a={A}&b={B}";
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return 0;
            }

            var responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return 0;
            }

            using (responseStream)
            using (var streamReader = new StreamReader(responseStream))
            {
                var json = streamReader.ReadToEnd();

                try
                {
                    long result = 0;
                    var numbers = JsonConvert.DeserializeObject<List<int>>(json);
                    foreach (var number in numbers)
                    {
                        result += number;
                    }

                    return result;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        private static void GetInput()
        {
            var input = ReadInput().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Host = input[0];
            Port = input[1];

            if(input.Length >= 3) A = input[2];
            if(input.Length >= 4) B = input[3];
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
