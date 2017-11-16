using System.Runtime.CompilerServices;

namespace VolatileDemo
{
    internal class Demo2
    {
        public bool Run()
        {
            var r1 = -1;
            var r2 = -1;

            var runner = new SimultaneousRunner(
                () => r1 = Processor0(),
                () => r2 = Processor1());

            runner.Run();

            return r1 == 0 && r2 == 0;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int Processor0()
        {
            x = 1;
            return y;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int Processor1()
        {
            y = 1;
            return x;
        }

        private int x;
        private int y;
    }
}