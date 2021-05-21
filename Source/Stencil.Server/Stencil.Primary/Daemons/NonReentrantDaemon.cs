using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Common.System;
using System;
using System.Threading;

namespace Stencil.Primary.Daemons
{
    public abstract class NonReentrantDaemon : ChokeableClass, IDaemonTask
    {
        /// <summary>
        /// Used to disable reentrancy.
        /// </summary>
        private long _executing = 0;
        private bool disposedValue;

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

#pragma warning disable CallBaseExecute // Call base.ExecuteMethod or base.ExecuteFunction when available
        public void Execute(IFoundation foundation, CancellationToken token)
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
                        this.ExecuteNonReentrant(foundation, token);
                    });
            }
            finally
            {
                Interlocked.Exchange(ref _executing, 0);
            }
        }
#pragma warning restore CallBaseExecute // Call base.ExecuteMethod or base.ExecuteFunction when available

        protected abstract void ExecuteNonReentrant(IFoundation foundation, CancellationToken token);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
