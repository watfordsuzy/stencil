using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.System;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Data.Sql;
using Stencil.Primary.Mapping;
using Stencil.Primary.UnitTests;
using System;
using System.Data.Entity.Core.EntityClient;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public abstract class BusinessTestBase : IDisposable
    {
        protected readonly EntityConnection _connection;
        protected readonly StencilContext _context;
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly Mock<IStencilContextFactory> _dataContextFactory;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;

        protected BusinessTestBase()
        {
            _connection = Effort.EntityConnectionFactory.CreateTransient("name=Test");
            _context = new StencilContext(_connection);

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _dataContextFactory = new Mock<IStencilContextFactory>();
            _dataContextFactory.Setup(dd => dd.CreateContext())
                               .Returns(_context);

            _container = new UnityContainer();
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IStencilContextFactory>(_dataContextFactory.Object);

            _foundation = new Mock<IFoundation>();
            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());

            Mapper.AddProfile<PrimaryMappingProfile>();
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
            _container.Dispose();
        }
    }
}
