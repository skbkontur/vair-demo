using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<BarrierVsInterlockedLongBenchmark>();
        }
    }
}