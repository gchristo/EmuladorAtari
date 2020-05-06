using System;
using System.Diagnostics;
using System.Threading;

namespace Emulator.Utils
{
    public class ProcessorCycle
    {
        public static void Cycle(int frequency, Action action)
        {
            var t = new Thread(() =>
            {
                int maxSleep = 1000 / frequency;

                var sw = Stopwatch.StartNew();
                while (true)
                {
                    action();

                    var sleep = maxSleep - (int)sw.ElapsedMilliseconds;
                    if (sleep > 0)
                    {
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        Console.WriteLine($"Can't sleep: {sleep}");
                    }
                    //GC.Collect();
                    sw.Restart();
                }
            });

            t.IsBackground = true;
            t.Start();
        }
    }
}
