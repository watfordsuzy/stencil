using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Common.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stencil.Primary.Daemons
{
    public abstract class NonReentrantDaemon : ChokeableClass, IDaemonTask
    {
        /// <summary>
        /// Used to disable reentrancy.
        /// </summary>
        private long _executing = 0;

        protected NonReentrantDaemon(IFoundation foundation)
            : base(foundation)
        {
        }

        protected NonReentrantDaemon(IFoundation foundation, IHandleExceptionProvider handleExceptionProvider)
            : base(foundation, handleExceptionProvider)
        {
        }

        public abstract DaemonSynchronizationPolicy SynchronizationPolicy { get; }

        public abstract string DaemonName { get; }

        /// <summary>
        /// Gets a value indicating whether or not the Daemon is executing.
        /// </summary>
        /// <remarks>
        /// This should not be used to control reentrancy.
        /// </remarks>
        protected bool IsExecuting => Interlocked.Read(ref _executing) != 0;

        public void Execute(IFoundation foundation)
        {
            if (0 != Interlocked.CompareExchange(ref _executing, 1, 0))
            {
                return;
            }

            try
            {
                base.ExecuteMethod(
                    nameof(Execute), 
                    delegate() 
                    {
                        this.ExecuteNonReentrant(foundation);
                    });
            }
            finally
            {
                Interlocked.Exchange(ref _executing, 0);
            }
        }

        protected abstract void ExecuteNonReentrant(IFoundation foundation);
    }
}
