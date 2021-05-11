using Codeable.Foundation.Common.Aspect;
using Codeable.Foundation.Common.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.UnitTests
{
    public class TestAspectCoordinator : IAspectCoordinator
    {
        public ChokePointResult EnterChokePoint(object invoker, EventArgs args) 
            => new ChokePointResult { Choke = false, };

        public ChokePointResult<TReturn> EnterChokePoint<TReturn>(object invoker, EventArgs args)
            => new ChokePointResult<TReturn> { Choke = false, };

        public ChokePointResult ExitChokePoint(object invoker, EventArgs args) 
            => new ChokePointResult { Choke = false, };

        public ChokePointResult<TReturn> ExitChokePoint<TReturn>(object invoker, EventArgs args)
            => new ChokePointResult<TReturn> { Choke = false, };

        public T WrapFunctionCall<T>(object invoker, string methodName, object[] parameters, bool forceThrow, IHandleExceptionProvider exceptionProvider, Func<T> function) 
            => function();

        public void WrapMethodCall(object invoker, string methodName, object[] parameters, bool forceThrow, IHandleExceptionProvider exceptionProvider, Action action)
            => action();
    }
}
