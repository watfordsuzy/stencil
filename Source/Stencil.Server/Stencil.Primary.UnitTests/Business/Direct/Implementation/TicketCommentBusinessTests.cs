using Microsoft.Practices.Unity;
using Moq;
using Stencil.Data.Sql;
using Stencil.Primary.Business.Index;
using Stencil.Primary.Synchronization;
using System;
using System.Collections.Generic;
using Xunit;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public class TicketCommentBusinessTests : BusinessTestBase
    {
        private readonly dm.Account _commenterAccount;
        private readonly dm.Account _otherAccount;
        private readonly dm.Ticket _ticket0;
        private readonly dm.TicketComment _ticketComment0;

        public TicketCommentBusinessTests()
            : base()
        {
            _commenterAccount = new dm.Account
            {
                account_id = Guid.NewGuid(),
            };
            _otherAccount = new dm.Account
            {
                account_id = Guid.NewGuid(),
            };
            _ticket0 = new dm.Ticket
            {
                ticket_id = Guid.NewGuid(),
                reported_by_id = _commenterAccount.account_id,
                assigned_to_id = _otherAccount.account_id,
            };
            _ticketComment0 = new dm.TicketComment
            {
                ticket_comment_id = Guid.NewGuid(),
                ticket_id = _ticket0.ticket_id,
                commenter_id = _commenterAccount.account_id,
            };

            _context.dbAccounts.Add(new dbAccount
            {
                account_id = _commenterAccount.account_id,
                email = "test0@example.com",
                password = "password",
                password_salt = "veruca",
                api_key = "key",
                api_secret = "secret",
            });
            _context.dbAccounts.Add(new dbAccount
            {
                account_id = _otherAccount.account_id,
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
            _context.dbTicketComments.Add(new dbTicketComment
            {
                ticket_comment_id = _ticketComment0.ticket_comment_id,
                ticket_id = _ticketComment0.ticket_id,
                commenter_id = _ticketComment0.commenter_id,
                commented_on_utc = DateTimeOffset.UtcNow,
                ticket_comment = "Hello World!",
            });
            _context.SaveChanges();
        }

        [Fact]
        public void CanAccountUpdateTicketComment_Requires_Ticket_Comment()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(_commenterAccount, Guid.Empty));

            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(_otherAccount, Guid.Empty));
        }

        [Fact]
        public void CanAccountUpdateTicketComment_Requires_Account()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Guid ticket_comment_id = Guid.NewGuid();
            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(null, ticket_comment_id));

            dm.Account account = new dm.Account
            {
                account_id = Guid.Empty,
            };
            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(account, ticket_comment_id));
        }

        [Fact]
        public void CanAccountUpdateTicketComment_Requires_Existing_Ticket_Comment()
        {
            Guid ticket_comment_id = Guid.NewGuid();

            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(_commenterAccount, ticket_comment_id));
        }

        [Fact]
        public void CanAccountUpdateTicketComment_Accepts_Ticket_Commenter()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Assert.True(ticketCommentBusiness.CanAccountUpdateTicketComment(_commenterAccount, _ticketComment0.ticket_comment_id));
        }

        [Fact]
        public void CanAccountUpdateTicketComment_Otherwise_Rejects_Account()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            var account = new dm.Account { account_id = Guid.NewGuid(), };

            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(account, _ticketComment0.ticket_comment_id));

            Assert.False(ticketCommentBusiness.CanAccountUpdateTicketComment(_otherAccount, _ticketComment0.ticket_comment_id));
        }

        [Fact]
        public void CanAccountDeleteTicketComment_Requires_Ticket()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Assert.False(ticketCommentBusiness.CanAccountDeleteTicketComment(_commenterAccount, Guid.Empty));

            Assert.False(ticketCommentBusiness.CanAccountDeleteTicketComment(_otherAccount, Guid.Empty));
        }

        [Fact]
        public void CanAccountDeleteTicketComment_Requires_Account()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Guid ticket_comment_id = Guid.NewGuid();
            Assert.False(ticketCommentBusiness.CanAccountDeleteTicketComment(null, ticket_comment_id));

            dm.Account account = new dm.Account
            {
                account_id = Guid.Empty,
            };
            Assert.False(ticketCommentBusiness.CanAccountDeleteTicketComment(account, ticket_comment_id));
        }

        [Fact]
        public void CanAccountDeleteTicketComment_Requires_Existing_Ticket_Comment()
        {
            Guid ticket_comment_id = Guid.NewGuid();

            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Assert.False(ticketCommentBusiness.CanAccountDeleteTicketComment(_commenterAccount, ticket_comment_id));
        }

        [Fact]
        public void CanAccountDeleteTicketComment_Accepts_Ticket_Commenter()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            Assert.True(ticketCommentBusiness.CanAccountDeleteTicketComment(_commenterAccount, _ticketComment0.ticket_comment_id));
        }

        [Fact]
        public void CanAccountDeleteTicketComment_Otherwise_Rejects_Account()
        {
            var ticketCommentBusiness = new TicketCommentBusiness(_foundation.Object);

            var account = new dm.Account { account_id = Guid.NewGuid(), };

            Assert.False(ticketCommentBusiness.CanAccountDeleteTicketComment(account, _ticketComment0.ticket_comment_id));
        }
    }
}
