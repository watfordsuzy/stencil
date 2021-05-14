using Microsoft.Practices.Unity;
using Moq;
using Stencil.Primary;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Business.Index;
using Stencil.SDK;
using System;
using System.Net.Http;
using System.Web.Http;
using Xunit;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public class TicketControllerTests : ControllerTestBase
    {
        [Fact]
        public void Create_Sets_ReportedBy_To_Current_Account()
        {
            dm.Ticket insertedTicket = null;
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.Insert(It.IsAny<dm.Ticket>()))
                          .Returns<dm.Ticket>(tt =>
                          {
                              // Capture the inserted ticket
                              insertedTicket = tt;
                              tt.ticket_id = Guid.NewGuid();
                              return tt;
                          });
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);
            var ticketIndex = new Mock<ITicketIndex>();
            ticketIndex.Setup(tt => tt.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(insertedTicket);
                           Assert.Equal(insertedTicket.ticket_id, id);

                           return insertedTicket.ToSDKModel();
                       });
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketController = CreatePost("tickets", _userAccount);

            var ticket = new sdk.Ticket
            {
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = sdk.TicketType.Bug,
            };

            var response = ticketController.Create(ticket);

            var httpResponse = HttpAssert.IsCreatedResponse(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.Ticket>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(insertedTicket);
            Assert.Equal(insertedTicket.ticket_id, itemResult.item.ticket_id);
            Assert.Equal(_userAccount.account_id, itemResult.item.reported_by_id);
        }

        [Fact]
        public void Update_Allows_Account_If_Can_Update()
        {
            dm.Ticket updatedTicket = null;
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(true);
            ticketBusiness.Setup(tt => tt.Update(It.IsAny<dm.Ticket>()))
                          .Returns<dm.Ticket>(tt =>
                          {
                              // Capture the updated ticket
                              updatedTicket = tt;
                              return tt;
                          });
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var ticketIndex = new Mock<ITicketIndex>();
            ticketIndex.Setup(tt => tt.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(updatedTicket);
                           Assert.Equal(updatedTicket.ticket_id, id);

                           return updatedTicket.ToSDKModel();
                       });
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            Guid ticket_id = Guid.NewGuid();
            var ticket = new sdk.Ticket
            {
                ticket_id = ticket_id,
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = sdk.TicketType.Bug,
            };

            var ticketController = CreatePut($"tickets/{ticket_id}", _userAccount);

            var response = ticketController.Update(ticket_id, ticket);

            var httpResponse = HttpAssert.IsSuccess(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.Ticket>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(updatedTicket);
            Assert.Equal(updatedTicket.ticket_id, itemResult.item.ticket_id);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
        }

        [Fact]
        public void Update_Allows_Admin_Account_If_Can_Cannot_Update()
        {
            dm.Ticket updatedTicket = null;
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            ticketBusiness.Setup(tt => tt.Update(It.IsAny<dm.Ticket>()))
                          .Returns<dm.Ticket>(tt =>
                          {
                              // Capture the updated ticket
                              updatedTicket = tt;
                              return tt;
                          });
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var ticketIndex = new Mock<ITicketIndex>();
            ticketIndex.Setup(tt => tt.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(updatedTicket);
                           Assert.Equal(updatedTicket.ticket_id, id);

                           return updatedTicket.ToSDKModel();
                       });
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            Guid ticket_id = Guid.NewGuid();
            var ticket = new sdk.Ticket
            {
                ticket_id = ticket_id,
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = sdk.TicketType.Bug,
            };

            var ticketController = CreatePut($"tickets/{ticket_id}", _adminAccount);

            var response = ticketController.Update(ticket_id, ticket);

            var httpResponse = HttpAssert.IsSuccess(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.Ticket>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(updatedTicket);
            Assert.Equal(updatedTicket.ticket_id, itemResult.item.ticket_id);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _adminAccount.account_id), ticket_id), Times.Once());
        }

        [Fact]
        public void Update_Rejects_NonAdmin_Account_If_Cannot_Update()
        {
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            Guid ticket_id = Guid.NewGuid();
            var ticket = new sdk.Ticket
            {
                ticket_id = ticket_id,
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = sdk.TicketType.Bug,
            };

            var ticketController = CreatePut($"tickets/{ticket_id}", _userAccount);

            var exception = Assert.Throws<HttpResponseException>(() => ticketController.Update(ticket_id, ticket));

            _ = HttpAssert.IsUnauthorizedResponse(exception.Response);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
        }

        [Fact]
        public void Delete_Allows_Account_If_Can_Delete()
        {
            Guid ticket_id = Guid.NewGuid();
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountDeleteTicket(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(true);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var ticket = new dm.Ticket
            {
                ticket_id = ticket_id,
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = dm.TicketType.Bug,
            };
            ticketBusiness.Setup(tt => tt.GetById(ticket_id))
                          .Returns<Guid>(id => ticket);

            var ticketController = CreateDelete($"tickets/{ticket_id}", _userAccount);

            var response = ticketController.Delete(ticket_id);

            var httpResponse = HttpAssert.IsSuccess(response);

            var actionResult = HttpAssert.IsActionResult(httpResponse.Content);
            Assert.True(actionResult.success);
            Assert.NotNull(actionResult.message);

            ticketBusiness.Verify(tt => tt.CanAccountDeleteTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
            ticketBusiness.Verify(tt => tt.Delete(ticket_id), Times.Once());
        }

        [Fact]
        public void Delete_Allows_Admin_Account_If_Can_Cannot_Delete()
        {
            Guid ticket_id = Guid.NewGuid();
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountDeleteTicket(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var ticket = new dm.Ticket
            {
                ticket_id = ticket_id,
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = dm.TicketType.Bug,
            };
            ticketBusiness.Setup(tt => tt.GetById(ticket_id))
                          .Returns<Guid>(id => ticket);

            var ticketController = CreateDelete($"tickets/{ticket_id}", _adminAccount);

            var response = ticketController.Delete(ticket_id);

            var httpResponse = HttpAssert.IsSuccess(response);

            var actionResult = HttpAssert.IsActionResult(httpResponse.Content);
            Assert.True(actionResult.success);
            Assert.NotNull(actionResult.message);

            ticketBusiness.Verify(tt => tt.CanAccountDeleteTicket(It.Is<dm.Account>(aa => aa.account_id == _adminAccount.account_id), ticket_id), Times.Once());
            ticketBusiness.Verify(tt => tt.Delete(ticket_id), Times.Once());
        }

        [Fact]
        public void Delete_Rejects_NonAdmin_Account_If_Cannot_Delete()
        {
            Guid ticket_id = Guid.NewGuid();
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountDeleteTicket(It.IsAny<dm.Account>(), It.IsAny<Guid>()))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var ticket = new dm.Ticket
            {
                ticket_id = ticket_id,
                ticket_title = "Title",
                ticket_description = "Description",
                ticket_type = dm.TicketType.Bug,
            };
            ticketBusiness.Setup(tt => tt.GetById(ticket_id))
                          .Returns<Guid>(id => ticket);

            var ticketController = CreateDelete($"tickets/{ticket_id}", _userAccount);

            var exception = Assert.Throws<HttpResponseException>(() => ticketController.Delete(ticket_id));

            _ = HttpAssert.IsUnauthorizedResponse(exception.Response);

            ticketBusiness.Verify(tt => tt.CanAccountDeleteTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
        }
    }
}
