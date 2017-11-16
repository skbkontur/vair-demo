using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [DisassemblyDiagnoser(printAsm: true, printSource: true, recursiveDepth: 0)]
    public class BarrierVsInterlockedLongBenchmark
    {
        [Benchmark]
        public long BareRead()
        {
            return data;
        }

        [Benchmark]
        public long VolatileRead()
        {
            return Volatile.Read(ref data);
        }

        [Benchmark]
        public long BarrierRead()
        {
            Thread.MemoryBarrier();

            return data;
        }

        [Benchmark]
        public long InterlockedRead()
        {
            return Interlocked.CompareExchange(ref data, 0, 0);
        }

        [Benchmark]
        public void BareWrite()
        {
            data = 42;
        }

        [Benchmark]
        public void VolatileWrite()
        {
            Volatile.Write(ref data, 42);
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

        private long data;
    }
}