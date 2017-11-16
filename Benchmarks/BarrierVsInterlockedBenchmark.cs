using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [DisassemblyDiagnoser(printAsm: true, printSource: true, recursiveDepth: 0)]
    public class BarrierVsInterlockedBenchmark
    {
        [Benchmark]
        public int BareRead()
        {
            return data;
        }

        [Benchmark]
        public int BarrierRead()
        {
            Thread.MemoryBarrier();

            return data;
        }

        [Benchmark]
        public int InterlockedRead()
        {
            return Interlocked.CompareExchange(ref data, 0, 0);
        }

        [Benchmark]
        public void BareWrite()
        {
            data = 42;
        }

        [Benchmark]
        public void BarrierWrite()
        {
            data = 42;

            Thread.MemoryBarrier();
        }

        [Benchmark]
        public void InterlockedWrite()
        {
            Interlocked.Exchange(ref data, 42);
        }

        private int data;
    }
}