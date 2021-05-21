using Microsoft.Practices.Unity;
using Moq;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Business.Index;
using System;
using System.Linq;
using Xunit;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Primary.Synchronization.Implementation
{
    public class TicketCommentSynchronizerTests : SynchronizerTestBase
    {
        [Fact]
        public void HydrateSDKModel_Uses_Index_When_Avaliable()
        {
            Guid commenter_id = Guid.NewGuid();
            sdk.Account commenter = new sdk.Account
            {
                account_id = commenter_id,
                first_name = "First",
                last_name = "Last",
                email = "first.last@example.com",
            };
            Guid ticket_comment_id = Guid.NewGuid();
            dm.TicketComment ticketComment = new dm.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = commenter_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };
            sdk.TicketComment updatedTicketComment = null;

            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.GetById(ticket_comment_id))
                                 .Returns(ticketComment);
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);
            var ticketCommentIndex = new Mock<ITicketCommentIndex>();
            ticketCommentIndex.Setup(tt => tt.UpdateDocument(It.IsAny<sdk.TicketComment>()))
                              .Callback<sdk.TicketComment>(tt => updatedTicketComment = tt)
                              .Returns(new IndexResult { success = true });
            _container.RegisterInstance<ITicketCommentIndex>(ticketCommentIndex.Object);
            var accountIndex = new Mock<IAccountIndex>();
            accountIndex.Setup(aa => aa.GetById(commenter_id))
                        .Returns(commenter);
            _container.RegisterInstance<IAccountIndex>(accountIndex.Object);

            var synchronizer = new TicketCommentSynchronizer(_foundation.Object);

            synchronizer.PerformSynchronizationForItem(ticket_comment_id);

            ticketCommentIndex.Verify(tt => tt.UpdateDocument(It.IsAny<sdk.TicketComment>()), Times.Once());
            Assert.NotNull(updatedTicketComment);
            Assert.Equal(commenter.email, updatedTicketComment.account_email);
            Assert.Equal($"{commenter.first_name} {commenter.last_name}", updatedTicketComment.account_name);
        }

        [Fact]
        public void HydrateSDKModel_Uses_SQL_When_Index_Unavaliable()
        {
            Guid commenter_id = Guid.NewGuid();
            dm.Account commenter = new dm.Account
            {
                account_id = commenter_id,
                first_name = "First",
                last_name = "Last",
                email = "first.last@example.com",
            };
            Guid ticket_comment_id = Guid.NewGuid();
            dm.TicketComment ticketComment = new dm.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = commenter_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };
            sdk.TicketComment updatedTicketComment = null;

            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.GetById(ticket_comment_id))
                                 .Returns(ticketComment);
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);
            var ticketCommentIndex = new Mock<ITicketCommentIndex>();
            ticketCommentIndex.Setup(tt => tt.UpdateDocument(It.IsAny<sdk.TicketComment>()))
                              .Callback<sdk.TicketComment>(tt => updatedTicketComment = tt)
                              .Returns(new IndexResult { success = true });
            _container.RegisterInstance<ITicketCommentIndex>(ticketCommentIndex.Object);
            var accountIndex = new Mock<IAccountIndex>();
            accountIndex.Setup(aa => aa.GetById(commenter_id))
                        .Returns((sdk.Account)null);
            _container.RegisterInstance<IAccountIndex>(accountIndex.Object);
            var accountBusiness = new Mock<IAccountBusiness>();
            accountBusiness.Setup(aa => aa.GetById(commenter_id))
                        .Returns(commenter);
            _container.RegisterInstance<IAccountBusiness>(accountBusiness.Object);

            var synchronizer = new TicketCommentSynchronizer(_foundation.Object);

            synchronizer.PerformSynchronizationForItem(ticket_comment_id);

            ticketCommentIndex.Verify(tt => tt.UpdateDocument(It.IsAny<sdk.TicketComment>()), Times.Once());
            Assert.NotNull(updatedTicketComment);
            Assert.Equal(commenter.email, updatedTicketComment.account_email);
            Assert.Equal($"{commenter.first_name} {commenter.last_name}", updatedTicketComment.account_name);
        }
    }
}
