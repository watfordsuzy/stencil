using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common.Configuration;
using Stencil.Data.Sql;
using Stencil.Primary.Mapping;
using Stencil.Primary.UnitTests;
using System;
using System.Data.Entity.Core.EntityClient;

namespace Stencil.Primary.Synchronization.Implementation
{
    public abstract class SynchronizerTestBase : IDisposable
    {
        protected readonly EntityConnection _connection;
        protected readonly TestStencilContext _context;
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly Mock<IStencilContextFactory> _dataContextFactory;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<ISettingsResolver> _settingsResolver;

        protected SynchronizerTestBase()
        {
            _connection = Effort.EntityConnectionFactory.CreateTransient("name=Test");
            _context = new TestStencilContext(_connection);

            _foundation = new Mock<IFoundation>();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _dataContextFactory = new Mock<IStencilContextFactory>();
            _dataContextFactory.Setup(dd => dd.CreateContext())
                               .Returns(_context);

            _settingsResolver = new Mock<ISettingsResolver>();

            _container = new UnityContainer();
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<IStencilContextFactory>(_dataContextFactory.Object);
            _container.RegisterInstance<ISettingsResolver>(_settingsResolver.Object);
            _container.RegisterInstance<StencilAPI>(new StencilAPI(_foundation.Object));

            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());

            Mapper.AddProfile<PrimaryMappingProfile>();
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.RealDispose();
            _container.Dispose();
        }
    }
}
