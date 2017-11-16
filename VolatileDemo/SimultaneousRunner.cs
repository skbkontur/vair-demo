using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VolatileDemo
{
    internal class SimultaneousRunner
    {
        public SimultaneousRunner(params Action[] actions)
        {
            trigger = new Trigger(actions.Length);
            tasks = new List<Task>(actions.Length);

            foreach (var action in actions)
            {
                tasks.Add(Task.Run(() =>
                {
                    trigger.SetReady();

                    action();
                }));
            }
        }

        public void Run()
        {
            trigger.StartWhenReady();

            Task.WaitAll(tasks.ToArray());
        }

        private readonly List<Task> tasks;

        private readonly Trigger trigger;
    }
}