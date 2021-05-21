using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common.Configuration;
using Stencil.Primary.Mapping;
using Stencil.Primary.UnitTests;
using System;

namespace Stencil.Primary.Workers
{
    public abstract class WorkerTestBase : IDisposable
    {
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<ISettingsResolver> _settingsResolver;
        protected readonly Mock<IDaemonManager> _daemonManager;

        protected WorkerTestBase()
        {
            _foundation = new Mock<IFoundation>();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _settingsResolver = new Mock<ISettingsResolver>();
            _daemonManager = new Mock<IDaemonManager>();

            _container = new UnityContainer();
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<ISettingsResolver>(_settingsResolver.Object);
            _container.RegisterInstance<StencilAPI>(new StencilAPI(_foundation.Object));
            _container.RegisterInstance<IDaemonManager>(_daemonManager.Object);

            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());
            _foundation.Setup(ff => ff.GetDaemonManager())
                       .Returns(_daemonManager.Object);

            Mapper.AddProfile<PrimaryMappingProfile>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
