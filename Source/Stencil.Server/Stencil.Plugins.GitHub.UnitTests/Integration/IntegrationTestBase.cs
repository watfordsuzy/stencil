using Codeable.Foundation.Common;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common.Configuration;
using Stencil.Plugins.GitHub.UnitTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Plugins.GitHub.Integration
{
    public abstract class IntegrationTestBase
    {
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<ISettingsResolver> _settingsResolver;

        protected IntegrationTestBase()
        {
            _foundation = new Mock<IFoundation>();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _settingsResolver = new Mock<ISettingsResolver>();

            _container = new UnityContainer();
            _container.RegisterInstance<IFoundation>(_foundation.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<ISettingsResolver>(_settingsResolver.Object);

            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());
        }
    }
}
