using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBApi.Connection
{
    internal sealed class ImmediateScheduler : TaskScheduler
    {
        private ImmediateScheduler()
        {
        }

        public static TaskScheduler Instance
        {
            get
            {
                return instance;
            }
        }

        protected override void QueueTask(Task task)
        {
            TryExecuteTask(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return null;
        }

        private static readonly ImmediateScheduler instance = new ImmediateScheduler();
    }
}
