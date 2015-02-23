using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lind.WPFTest.ViewModels
{
    public class CancelableTask : Task
    {
        protected CancelableTask(Action action, CancellationTokenSource source)
            : base(action, source.Token)
        {
            Source = source;
        }

        public CancellationToken Token { get { return Source.Token; } }
        public void Cancel(bool throwOnFirstException = true)
        {
            this.Source.Cancel(throwOnFirstException);
        }
        private CancellationTokenSource Source { get; set; }
        public static CancelableTask Run(Action<CancellationToken> action)
        {
            CancelableTask task = Create(action);
            task.Start();
            return task;
        }
        public static CancelableTask Create(Action<CancellationToken> action)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancelableTask task = new CancelableTask(() => action(source.Token), source);
            return task;
        }
    }
}
