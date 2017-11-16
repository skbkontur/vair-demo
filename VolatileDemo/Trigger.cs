using System.Threading;

namespace VolatileDemo
{
    internal class Trigger
    {
        public Trigger(int count)
        {
            ready = new CountdownEvent(count);
        }

        public void StartWhenReady()
        {
            ready.Wait();
            trigger.Set();
        }

        public void SetReady()
        {
            ready.Signal();
            trigger.Wait();
        }

        private readonly CountdownEvent ready;
        private readonly ManualResetEventSlim trigger = new ManualResetEventSlim();
    }
}