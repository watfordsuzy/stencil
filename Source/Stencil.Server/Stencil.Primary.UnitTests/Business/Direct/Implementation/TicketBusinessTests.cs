using Microsoft.Practices.Unity;
using Moq;
using Stencil.Data.Sql;
using Stencil.Primary.Business.Index;
using System;
using Xunit;

using dm = Stencil.Domain;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public class TicketBusinessTests : BusinessTestBase
    {
        private readonly dm.Account _reportedByAccount;
        private readonly dm.Account _assignedToAccount;
        private readonly dm.Ticket _ticket0;
        private readonly dm.Product _product0;

        public TicketBusinessTests()
            : base()
        {
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
                reported_by_id = _reportedByAccount.account_id,
                assigned_to_id = _assignedToAccount.account_id,
            };
            _product0 = new dm.Product
            {
                product_id = Guid.NewGuid(),
                product_owner_id = _reportedByAccount.account_id,
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
                reported_by_id = _ticket0.reported_by_id,
                assigned_to_id = _ticket0.assigned_to_id,
                ticket_title = "Title",
                ticket_description = "Description",
                opened_on_utc = DateTimeOffset.UtcNow,
                ticket_type = (int)dm.TicketType.Feature,
                ticket_status = (int)dm.TicketStatus.Open,
            });
            _context.dbProducts.Add(new dbProduct
            {
                product_id = _product0.product_id,
                product_name = "Product 0",
                product_owner_id = _product0.product_owner_id,
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

        [Fact]
        public void GetAssignee_Returns_Null_If_Invalid_Ticket()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.Null(ticketBusiness.GetAssignee(Guid.Empty));

            Assert.Null(ticketBusiness.GetAssignee(Guid.NewGuid()));
        }

        [Fact]
        public void GetAssignee_Returns_Assignee_Of_Ticket()
        {
            var ticketBusiness = new TicketBusiness(_foundation.Object);

            Assert.Equal(_assignedToAccount.account_id, ticketBusiness.GetAssignee(_ticket0.ticket_id));
        }

        [Fact]
        public void AssignToProductOwner_Does_Nothing_If_Missing_Ticket()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            ticketBusiness.AssignToProductOwner(Guid.Empty, _product0.product_id);

            ticketIndex.Verify(tt => tt.UpdateAssignedTo(It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Never());

            ticketBusiness.AssignToProductOwner(Guid.NewGuid(), _product0.product_id);

            ticketIndex.Verify(tt => tt.UpdateAssignedTo(It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Never());
        }

        [Fact]
        public void AssignToProductOwner_Does_Nothing_If_Missing_Product()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            ticketBusiness.AssignToProductOwner(_ticket0.ticket_id, Guid.Empty);

            ticketIndex.Verify(tt => tt.UpdateAssignedTo(It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Never());

            ticketBusiness.AssignToProductOwner(_ticket0.ticket_id, Guid.NewGuid());

            ticketIndex.Verify(tt => tt.UpdateAssignedTo(It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Never());
        }

        [Fact]
        public void AssignToProductOwner_Assigns_Ticket_To_Product_Owner()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            ticketBusiness.AssignToProductOwner(_ticket0.ticket_id, _product0.product_id);

            dbTicket ticket = _context.dbTickets.Find(_ticket0.ticket_id);
            Assert.Equal(_product0.product_owner_id, ticket.assigned_to_id);

            ticketIndex.Verify(tt => tt.UpdateAssignedTo(_ticket0.ticket_id, _product0.product_owner_id), Times.Once());
        }
    }
}
