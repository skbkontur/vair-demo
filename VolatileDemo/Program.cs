using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace VolatileDemo
{
    internal class Program
    {
        public static void Main()
        {
            ConsoleHelpers.SetupConsole();

            RunDemo1(false);
        }

        private static void RunDemo1(bool disassemble)
        {
            Console.WriteLine("Iterations: " + new Demo1().WaitForData());

            if (disassemble)
                Process.Start("Disassembler.exe", Process.GetCurrentProcess().Id + " " + typeof(Demo1).FullName).WaitForExit();
        }

        private static void RunDemo2(bool disassemble)
        {
            var runs = 0L;
            var bothZero = 0L;

            var running = true;
            Task.Run(() =>
            {
                Console.ReadKey(true);
                running = false;
            });

            while (running)
            {
                runs++;
                if (new Demo2().Run())
                    bothZero++;

                if (runs % 1000 == 0)
                    Console.Write("\rBoth zero %: {0:f6}", bothZero * 100.0 / runs);
            }

            if (disassemble)
                Process.Start("Disassembler.exe", Process.GetCurrentProcess().Id + " " + typeof(Demo2).FullName).WaitForExit();
        }
    }
}
