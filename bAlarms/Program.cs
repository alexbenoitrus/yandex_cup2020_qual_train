namespace Yandex.Cup2020.Qual.Train.B.Alarms
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";

        private static int WakeupAlarmNumber;
        private static int PauseSize;

        private static IEnumerable<int> Starts;

        static void Main(string[] args)
        {
            GetInput();
            WriteOutput(Process());
        }

        private static int Process()
        {
            int firstAlarm = Starts.Min();
            int maxBellTime = firstAlarm + (WakeupAlarmNumber - 1) * PauseSize;

            if (WakeupAlarmNumber == 1) return firstAlarm;

            var bells = new List<int>();
            foreach (var start in Starts)
            {
                for (int i = 0; i < WakeupAlarmNumber; i++)
                {
                    var bellTime = start + (i * PauseSize);
                    if (bellTime > maxBellTime) break;

                    bells.Add(bellTime);
                }
            }

            bells = bells.Distinct().ToList();
            bells.Sort();

            return bells[WakeupAlarmNumber - 1] % 60;
        }

        private static void GetInput()
        {
            var input = ReadInput().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var settings = input[0].Split(' ').Select(int.Parse).ToArray();
            Starts = input[1].Split(' ').Select(int.Parse);

            PauseSize = settings[1];
            WakeupAlarmNumber = settings[2];
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