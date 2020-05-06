using System;
using System.Diagnostics;

namespace Emulator.Utils
{
    public class Benchmark
    {
        public static void Log(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            Console.WriteLine(sw.ElapsedTicks);
        }
    }
}
