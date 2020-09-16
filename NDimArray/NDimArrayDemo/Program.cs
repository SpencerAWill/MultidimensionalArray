using NDimArray;
using NDimArrayDemo.Demos;
using System;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace NDimArrayDemo
{
    class Program
    {
        private static bool EnablePerElementWriting = true;

        static void Main(string[] args)
        {
            int[] dims = (int[])Array.CreateInstance(typeof(int), 20);
            for (int i = 0; i < dims.Length; i++)
            {
                dims[i] = 2;
            }

            var array = new NDimArray<string>(dims);

            Console.WriteLine($"Dimensions: [{string.Join(", ", dims.Select(x => x.ToString()).ToArray())}]");
            Console.WriteLine($"Total items: {array.Length}");
            Console.WriteLine("Calculating estimated time...");
            Console.WriteLine($"Est time: {GetEstimatedTime(array, 10)}");
            Console.WriteLine("Press any key to enumerate...");
            Console.ReadLine();
            Stopwatch s = Stopwatch.StartNew();
            array.Enumerate((index, item) => { if (EnablePerElementWriting) { Console.WriteLine($"[{string.Join(", ", index)}]"); } });
            s.Stop();

            Console.WriteLine("==========");
            Console.WriteLine($"{s.Elapsed} elapsed.");
            Console.ReadLine();
        }

        private static TimeSpan GetEstimatedTime(NDimArray<string> array, int samples)
        {
            var estSeconds = AvgTimeFor1MOps(samples, out var milArray) / milArray.Length * array.Length;
            return TimeSpan.FromSeconds(estSeconds);
        }

        private static double AvgTimeFor1MOps(int samples, out NDimArray<int> milArray)
        {
            milArray = new NDimArray<int>(new int[] { 10, 10, 10, 10, 10, 10 });
            var execTimes = new double[samples];

            for (int i = 0; i < execTimes.Length; i++)
            {
                Stopwatch s = Stopwatch.StartNew();
                milArray.Enumerate(x => { });
                s.Stop();
                execTimes[i] = s.Elapsed.TotalSeconds;
            }

            return execTimes.Average();
        }
    }
}
