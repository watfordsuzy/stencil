using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using Codeable.Foundation.Common.System;
using Effort.Provider;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Data.Sql;
using Stencil.Primary.UnitTests;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using Xunit;

using dm = Stencil.Domain;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public class TicketBusinessTests : IDisposable
    {
        private readonly EntityConnection _connection;
        private readonly StencilContext _context;
        private readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        private readonly Mock<IStencilContextFactory> _dataContextFactory;
        private readonly UnityContainer _container;
        private readonly Mock<IFoundation> _foundation;

        private readonly dm.Account _reportedByAccount;
        private readonly dm.Account _assignedToAccount;
        private readonly dm.Ticket _ticket0;

        public TicketBusinessTests()
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

            _reportedByAccount = new dm.Account
            {
                account_id = Guid.NewGuid(),
            };
            _assignedToAccount = new dm.Account
            {
                account_id = Guid.NewGuid(),
            };
            _ticket0 = new dm.Ticket
            {
                ticket_id = Guid.NewGuid(),
            };

            _context.dbAccounts.Add(new dbAccount
            {
                account_id = _reportedByAccount.account_id,
                email = "test0@example.com",
                password = "password",
                password_salt = "veruca",
                api_key = "key",
                api_secret = "secret",
            });
            _context.dbAccounts.Add(new dbAccount
            {
                account_id = _assignedToAccount.account_id,
                email = "test1@example.com",
                password = "password",
                password_salt = "veruca",
                api_key = "key",
                api_secret = "secret",
            });
            _context.dbTickets.Add(new dbTicket
            {
                ticket_id = _ticket0.ticket_id,
                reported_by_id = _reportedByAccount.account_id,
                assigned_to_id = _assignedToAccount.account_id,
                ticket_title = "Title",
                ticket_description = "Description",
                opened_on_utc = DateTimeOffset.UtcNow,
                ticket_type = (int)dm.TicketType.Feature,
                ticket_status = (int)dm.TicketStatus.Open,
            });
            _context.SaveChanges();
        }

        [Fact]
        public void CanAccountUpdateTicket_Requires_Ticket()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.False(ticketBusiness.CanAccountUpdateTicket(_reportedByAccount, Guid.Empty));

            Assert.False(ticketBusiness.CanAccountUpdateTicket(_assignedToAccount, Guid.Empty));
        }

        [Fact]
        public void CanAccountUpdateTicket_Requires_Account()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Guid ticket_id = Guid.NewGuid();
            Assert.False(ticketBusiness.CanAccountUpdateTicket(null, ticket_id));

            dm.Account account = new dm.Account
            {
                account_id = Guid.Empty,
            };
            Assert.False(ticketBusiness.CanAccountUpdateTicket(account, ticket_id));
        }

        [Fact]
        public void CanAccountUpdateTicket_Requires_Existing_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.False(ticketBusiness.CanAccountUpdateTicket(_reportedByAccount, ticket_id));
        }

        [Fact]
        public void CanAccountUpdateTicket_Accepts_Ticket_Reporter()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.True(ticketBusiness.CanAccountUpdateTicket(_reportedByAccount, _ticket0.ticket_id));
        }

        [Fact]
        public void CanAccountUpdateTicket_Accepts_Ticket_Owner()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.True(ticketBusiness.CanAccountUpdateTicket(_assignedToAccount, _ticket0.ticket_id));
        }

        [Fact]
        public void CanAccountUpdateTicket_Otherwise_Rejects_Account()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            var account = new dm.Account { account_id = Guid.NewGuid(), };

            Assert.False(ticketBusiness.CanAccountUpdateTicket(account, _ticket0.ticket_id));
        }


        [Fact]
        public void CanAccountDeleteTicket_Requires_Ticket()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.False(ticketBusiness.CanAccountDeleteTicket(_reportedByAccount, Guid.Empty));

            Assert.False(ticketBusiness.CanAccountDeleteTicket(_assignedToAccount, Guid.Empty));
        }

        [Fact]
        public void CCanAccountDeleteTicket_Requires_Account()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Guid ticket_id = Guid.NewGuid();
            Assert.False(ticketBusiness.CanAccountDeleteTicket(null, ticket_id));

            dm.Account account = new dm.Account
            {
                account_id = Guid.Empty,
            };
            Assert.False(ticketBusiness.CanAccountDeleteTicket(account, ticket_id));
        }

        [Fact]
        public void CanAccountDeleteTicket_Requires_Existing_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.False(ticketBusiness.CanAccountDeleteTicket(_reportedByAccount, ticket_id));
        }

        [Fact]
        public void CanAccountDeleteTicket_Rejects_Ticket_Reporter()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.False(ticketBusiness.CanAccountDeleteTicket(_reportedByAccount, _ticket0.ticket_id));
        }

        [Fact]
        public void CanAccountDeleteTicket_Accepts_Ticket_Owner()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.True(ticketBusiness.CanAccountDeleteTicket(_assignedToAccount, _ticket0.ticket_id));
        }

        [Fact]
        public void CanAccountDeleteTicket_Otherwise_Rejects_Account()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            var account = new dm.Account { account_id = Guid.NewGuid(), };

            Assert.False(ticketBusiness.CanAccountDeleteTicket(account, _ticket0.ticket_id));
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
            _container.Dispose();
        }
    }
}
