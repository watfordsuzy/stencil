using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Stencil.Primary.Daemons
{
    public class NonReentrantDaemonTests : DaemonTestBase
    {
        [Fact]
        public async Task Execute_Does_Not_Allow_Reentrancy()
        {
            var daemon0 = new TestNonReentrantDaemon(_foundation.Object);

            long initialValue = daemon0.Value;

            // Don't let this test go on forever
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            long taskCount = 0;
            var task0 = Task.Run(() =>
            {
                Interlocked.Increment(ref taskCount);
                daemon0.Execute(_foundation.Object, cts.Token);
            });
            var task1 = Task.Run(() =>
            {
                Interlocked.Increment(ref taskCount);
                daemon0.Execute(_foundation.Object, cts.Token);
            });

            //
            // Wait for the two tasks to actually start
            //
            // CAW: we need to do this for the test, because of a race
            //      condition introduced spawning tasks.
            //
            //      If task0 launches, waits, and receives the GO below
            //      before task1 gets off the ground. task1 will be stuck
            //      on its own Wait and Task.WhenAll will never complete
            //      before the cancellation token throws.
            //
            while (2 != Interlocked.Read(ref taskCount))
            {
                await Task.Yield();
            }

            // Allow only one of the daemons to continue (if they are unexpectedly reentrant)
            daemon0.Go();

            // In the "happy" path this will always succeed within the cancellation time
            await Task.WhenAll(task0, task1);

            // Ensure only one Increment ocurred, e.g. the method was not reentrant
            Assert.Equal(initialValue + 1, daemon0.Value);
        }

        [Fact]
        public async Task IsExecuting_Detects_When_Executing()
        {
            var daemon0 = new TestNonReentrantDaemon(_foundation.Object);

            long initialValue = daemon0.Value;

            // Don't let this test go on forever
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            var task0 = Task.Run(() => daemon0.Execute(_foundation.Object, cts.Token));
            
            // Ensure that the daemon begins execution at some point
            while (!daemon0.IsExecutingCore)
            {
                cts.Token.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            // We should now be executing
            Assert.True(daemon0.IsExecutingCore);

            // Allow the daemons to continue
            daemon0.Go();

            await task0;

            // We should no longer be executing
            Assert.False(daemon0.IsExecutingCore);

            // Ensure only one Increment ocurred, e.g. the method was not reentrant
            Assert.Equal(initialValue + 1, daemon0.Value);
        }

        [Fact]
        public async Task Execute_Works_If_Back_To_Back()
        {
            var daemon0 = new TestNonReentrantDaemon(_foundation.Object);

            long initialValue = daemon0.Value;

            // Don't let this test go on forever
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            var task0 = Task.Run(() => daemon0.Execute(_foundation.Object, cts.Token));

            // Allow the daemon to continue
            daemon0.Go();

            await task0;

            // Ensure the daemon succeeded
            Assert.Equal(initialValue + 1, daemon0.Value);

            var task1 = Task.Run(() => daemon0.Execute(_foundation.Object, cts.Token));

            // Allow the daemon to continue
            daemon0.Go();

            await task1;

            // Ensure the daemon succeeded
            Assert.Equal(initialValue + 2, daemon0.Value);
        }

        [Fact]
        public async Task Execute_Works_Back_To_Back_Even_With_Exceptions()
        {
            var daemon0 = new TestNonReentrantDaemon(_foundation.Object);

            long initialValue = daemon0.Value;

            // Don't let this test go on forever
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            // Throw an exception while executing the first time
            daemon0.ThrowOnceOnExecute = true;

            var task0 = Task.Run(() => daemon0.Execute(_foundation.Object, cts.Token));

            // Wait for the daemon to throw an exception
            await Assert.ThrowsAsync<TestNonReentrantDaemonException>(async () => await task0);

            // Ensure the daemon did not succeed
            Assert.Equal(initialValue, daemon0.Value);

            var task1 = Task.Run(() => daemon0.Execute(_foundation.Object, cts.Token));

            // Allow the daemon to continue
            daemon0.Go();

            await task1;

            // Ensure the daemon succeeded
            Assert.Equal(initialValue + 1, daemon0.Value);
        }

        internal class TestNonReentrantDaemon : NonReentrantDaemon
        {
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

            private long _value = 0;

            public long Value => _value;

            public bool ThrowOnceOnExecute
            {
                get;
                set;
            }

            public override DaemonSynchronizationPolicy SynchronizationPolicy => DaemonSynchronizationPolicy.SingleAppDomain;

            public override string DaemonName => nameof(TestNonReentrantDaemon);

            public bool IsExecutingCore => base.IsExecuting;

            public TestNonReentrantDaemon(IFoundation foundation)
                : base(foundation)
            {
            }

            public void Go() => _semaphore.Release();

            protected override void ExecuteNonReentrant(IFoundation foundation, CancellationToken token)
            {
                if (ThrowOnceOnExecute)
                {
                    ThrowOnceOnExecute = false;
                    throw new TestNonReentrantDaemonException();
                }

                Interlocked.Increment(ref _value);

                _semaphore.Wait(token);
            }
        }

        internal class TestNonReentrantDaemonException : Exception
        {
        }
    }
}
