using Microsoft.Practices.Unity;
using Moq;
using Stencil.Primary;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Business.Index;
using Stencil.SDK;
using System;
using System.Web.Http;
using Xunit;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public class TicketCommentControllerTests : ControllerTestBase<TicketCommentController>
    {
        protected override TicketCommentController CreateController() => new(_foundation.Object);

        [Fact]
        public void Create_Sets_Commenter_To_Current_Account()
        {
            DateTime now = DateTime.UtcNow;

            dm.TicketComment insertedTicketComment = null;
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.Insert(It.IsAny<dm.TicketComment>()))
                          .Returns<dm.TicketComment>(tt =>
                          {
                              // Capture the inserted ticket
                              insertedTicketComment = tt;
                              tt.ticket_comment_id = Guid.NewGuid();
                              return tt;
                          });
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);
            var ticketCommentIndex = new Mock<ITicketCommentIndex>();
            ticketCommentIndex.Setup(tt => tt.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(insertedTicketComment);
                           Assert.Equal(insertedTicketComment.ticket_comment_id, id);

                           return insertedTicketComment.ToSDKModel();
                       });
            _container.RegisterInstance<ITicketCommentIndex>(ticketCommentIndex.Object);

            var ticketCommentController = CreatePost("ticketcomments", _userAccount);

            var ticketComment = new sdk.TicketComment
            {
                ticket_id = Guid.NewGuid(),
                ticket_comment ="Hello World!",
            };

            var response = ticketCommentController.Create(ticketComment);

            var httpResponse = HttpAssert.IsCreatedResponse(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.TicketComment>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(insertedTicketComment);
            Assert.Equal(insertedTicketComment.ticket_id, itemResult.item.ticket_id);
            Assert.Equal(insertedTicketComment.ticket_comment, itemResult.item.ticket_comment);
            Assert.Equal(_userAccount.account_id, itemResult.item.commenter_id);
            Assert.True(now <= itemResult.item.commented_on_utc);
        }


        [Fact]
        public void Update_Allows_Account_If_Can_Update()
        {
            dm.TicketComment updatedTicket = null;
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.CanAccountUpdateTicketComment(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(true);
            ticketCommentBusiness.Setup(tt => tt.Update(It.IsAny<dm.TicketComment>()))
                          .Returns<dm.TicketComment>(tt =>
                          {
                              // Capture the updated ticket
                              updatedTicket = tt;
                              return tt;
                          });
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);

            var ticketIndex = new Mock<ITicketCommentIndex>();
            ticketIndex.Setup(tt => tt.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(updatedTicket);
                           Assert.Equal(updatedTicket.ticket_comment_id, id);

                           return updatedTicket.ToSDKModel();
                       });
            _container.RegisterInstance<ITicketCommentIndex>(ticketIndex.Object);

            Guid ticket_comment_id = Guid.NewGuid();
            var ticketComment = new sdk.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = _userAccount.account_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };

            var ticketCommentController = CreatePut($"ticketcomments/{ticket_comment_id}", _userAccount);

            var response = ticketCommentController.Update(ticket_comment_id, ticketComment);

            var httpResponse = HttpAssert.IsSuccess(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.TicketComment>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(updatedTicket);
            Assert.Equal(updatedTicket.ticket_comment_id, itemResult.item.ticket_comment_id);

            ticketCommentBusiness.Verify(tt => tt.CanAccountUpdateTicketComment(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_comment_id), Times.Once());
        }

        [Fact]
        public void Update_Allows_Admin_Account_If_Can_Cannot_Update()
        {
            dm.TicketComment updatedTicket = null;
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.CanAccountUpdateTicketComment(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            ticketCommentBusiness.Setup(tt => tt.Update(It.IsAny<dm.TicketComment>()))
                          .Returns<dm.TicketComment>(tt =>
                          {
                              // Capture the updated ticket
                              updatedTicket = tt;
                              return tt;
                          });
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);

            var ticketIndex = new Mock<ITicketCommentIndex>();
            ticketIndex.Setup(tt => tt.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(updatedTicket);
                           Assert.Equal(updatedTicket.ticket_comment_id, id);

                           return updatedTicket.ToSDKModel();
                       });
            _container.RegisterInstance<ITicketCommentIndex>(ticketIndex.Object);

            Guid ticket_comment_id = Guid.NewGuid();
            var ticketComment = new sdk.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = Guid.NewGuid(),
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };

            var ticketCommentController = CreatePut($"ticketcomments/{ticket_comment_id}", _adminAccount);

            var response = ticketCommentController.Update(ticket_comment_id, ticketComment);

            var httpResponse = HttpAssert.IsSuccess(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.TicketComment>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(updatedTicket);
            Assert.Equal(updatedTicket.ticket_comment_id, itemResult.item.ticket_comment_id);

            ticketCommentBusiness.Verify(tt => tt.CanAccountUpdateTicketComment(It.Is<dm.Account>(aa => aa.account_id == _adminAccount.account_id), ticket_comment_id), Times.Once());
        }

        [Fact]
        public void Update_Rejects_NonAdmin_Account_If_Cannot_Update()
        {
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.CanAccountUpdateTicketComment(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);

            Guid ticket_comment_id = Guid.NewGuid();
            var ticketComment = new sdk.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = _userAccount.account_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };

            var ticketCommentController = CreatePut($"ticketcomments/{ticket_comment_id}", _userAccount);

            var exception = Assert.Throws<HttpResponseException>(() => ticketCommentController.Update(ticket_comment_id, ticketComment));

            _ = HttpAssert.IsUnauthorizedResponse(exception.Response);

            ticketCommentBusiness.Verify(tt => tt.CanAccountUpdateTicketComment(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_comment_id), Times.Once());
        }

        [Fact]
        public void Delete_Allows_Account_If_Can_Delete()
        {
            Guid ticket_comment_id = Guid.NewGuid();
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.CanAccountDeleteTicketComment(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(true);
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);

            var ticketComment = new dm.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = _userAccount.account_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };
            ticketCommentBusiness.Setup(tt => tt.GetById(ticket_comment_id))
                          .Returns<Guid>(id => ticketComment);

            var ticketCommentController = CreateDelete($"ticketcomments/{ticket_comment_id}", _userAccount);

            var response = ticketCommentController.Delete(ticket_comment_id);

            var httpResponse = HttpAssert.IsSuccess(response);

            var actionResult = HttpAssert.IsActionResult(httpResponse.Content);
            Assert.True(actionResult.success);
            Assert.NotNull(actionResult.message);

            ticketCommentBusiness.Verify(tt => tt.CanAccountDeleteTicketComment(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_comment_id), Times.Once());
            ticketCommentBusiness.Verify(tt => tt.Delete(ticket_comment_id), Times.Once());
        }

        [Fact]
        public void Delete_Allows_Admin_Account_If_Can_Cannot_Delete()
        {
            Guid ticket_comment_id = Guid.NewGuid();
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.CanAccountDeleteTicketComment(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);

            var ticketComment = new dm.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = _userAccount.account_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };
            ticketCommentBusiness.Setup(tt => tt.GetById(ticket_comment_id))
                          .Returns<Guid>(id => ticketComment);

            var ticketCommentController = CreateDelete($"ticketcomments/{ticket_comment_id}", _adminAccount);

            var response = ticketCommentController.Delete(ticket_comment_id);

            var httpResponse = HttpAssert.IsSuccess(response);

            var actionResult = HttpAssert.IsActionResult(httpResponse.Content);
            Assert.True(actionResult.success);
            Assert.NotNull(actionResult.message);

            ticketCommentBusiness.Verify(tt => tt.CanAccountDeleteTicketComment(It.Is<dm.Account>(aa => aa.account_id == _adminAccount.account_id), ticket_comment_id), Times.Once());
            ticketCommentBusiness.Verify(tt => tt.Delete(ticket_comment_id), Times.Once());
        }

        [Fact]
        public void Delete_Rejects_NonAdmin_Account_If_Cannot_Delete()
        {
            Guid ticket_comment_id = Guid.NewGuid();
            var ticketCommentBusiness = new Mock<ITicketCommentBusiness>();
            ticketCommentBusiness.Setup(tt => tt.CanAccountDeleteTicketComment(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            _container.RegisterInstance<ITicketCommentBusiness>(ticketCommentBusiness.Object);

            var ticketComment = new dm.TicketComment
            {
                ticket_comment_id = ticket_comment_id,
                ticket_id = Guid.NewGuid(),
                commenter_id = _userAccount.account_id,
                commented_on_utc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                ticket_comment = "Hello World!",
            };
            ticketCommentBusiness.Setup(tt => tt.GetById(ticket_comment_id))
                          .Returns<Guid>(id => ticketComment);

            var ticketCommentController = CreateDelete($"ticketcomments/{ticket_comment_id}", _userAccount);

            var exception = Assert.Throws<HttpResponseException>(() => ticketCommentController.Delete(ticket_comment_id));

            _ = HttpAssert.IsUnauthorizedResponse(exception.Response);

            ticketCommentBusiness.Verify(tt => tt.CanAccountDeleteTicketComment(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_comment_id), Times.Once());
        }
    }
}
