using System.Threading.Tasks;

namespace VolatileDemo
{
    internal class Demo1
    {
        public int WaitForData()
        {
            Task.Delay(1).ContinueWith(_ => data = 1);

            var iterationsPassed = 0;

            while (data == 0)
                iterationsPassed++;

            return iterationsPassed;
        }

        private int data;
    }
}