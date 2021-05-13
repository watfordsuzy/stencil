using Stencil.Data.Sql;
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
    }
}
