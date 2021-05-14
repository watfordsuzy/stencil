using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Data.Sql;
using Stencil.Primary.Business.Integration;
using Stencil.Primary.Mapping;
using Stencil.Primary.UnitTests;
using System;
using System.Data.Entity.Core.EntityClient;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public abstract class BusinessTestBase : IDisposable
    {
        protected readonly EntityConnection _connection;
        protected readonly TestStencilContext _context;
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly Mock<IStencilContextFactory> _dataContextFactory;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<IDependencyCoordinator> _dependencyCoordinator;

        protected BusinessTestBase()
        {
            _connection = Effort.EntityConnectionFactory.CreateTransient("name=Test");
            _context = new TestStencilContext(_connection);

            _container = new UnityContainer();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _dataContextFactory = new Mock<IStencilContextFactory>();
            _dataContextFactory.Setup(dd => dd.CreateContext())
                               .Returns(_context);
            _dependencyCoordinator = new Mock<IDependencyCoordinator>();

            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<IStencilContextFactory>(_dataContextFactory.Object);
            _container.RegisterInstance<IDependencyCoordinator>(_dependencyCoordinator.Object);

            _foundation = new Mock<IFoundation>();
            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());

            _container.RegisterInstance<StencilAPI>(new StencilAPI(_foundation.Object));

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
