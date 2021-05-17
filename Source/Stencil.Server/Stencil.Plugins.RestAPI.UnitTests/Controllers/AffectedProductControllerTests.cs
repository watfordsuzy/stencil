using Codeable.Foundation.Common;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Primary;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Business.Index;
using Stencil.Primary.Workers;
using Stencil.Primary.Workers.Models;
using Stencil.SDK;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Xunit;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public class AffectedProductControllerTests : ControllerTestBase<AffectedProductController>
    {
        protected override AffectedProductController CreateController() => new(_foundation.Object);

        [Fact]
        public void Create_Allows_Account_If_Can_Update_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();
            dm.AffectedProduct insertedAffectedProduct = null;
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.Insert(It.IsAny<dm.AffectedProduct>()))
                          .Returns<dm.AffectedProduct>(ap =>
                          {
                              // Capture the inserted value
                              insertedAffectedProduct = ap;
                              ap.affected_product_id = Guid.NewGuid();
                              return ap;
                          });
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var affectedProductIndex = new Mock<IAffectedProductIndex>();
            affectedProductIndex.Setup(ap => ap.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(insertedAffectedProduct);
                           Assert.Equal(insertedAffectedProduct.affected_product_id, id);

                           return insertedAffectedProduct.ToSDKModel();
                       });
            _container.RegisterInstance<IAffectedProductIndex>(affectedProductIndex.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(true);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var assignProductOwnerWorker = new TestAssignProductOwnerWorker(_foundation.Object);
            _daemonManager.Setup(dd => dd.GetRegisteredDaemonTask(nameof(AssignProductOwnerWorker)))
                          .Returns(assignProductOwnerWorker);

            var affectedProductController = CreatePost("affectedproducts", _userAccount);

            var affectedProduct = new sdk.AffectedProduct
            {
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };

            var response = affectedProductController.Create(affectedProduct);

            var httpResponse = HttpAssert.IsCreatedResponse(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.AffectedProduct>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(insertedAffectedProduct);
            Assert.Equal(insertedAffectedProduct.affected_product_id, itemResult.item.affected_product_id);
            Assert.Equal(affectedProduct.ticket_id, itemResult.item.ticket_id);
            Assert.Equal(affectedProduct.product_id, itemResult.item.product_id);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
        }

        [Fact]
        public void Create_Allows_Admin_Account_If_Cannot_Update_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();
            dm.AffectedProduct insertedAffectedProduct = null;
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.Insert(It.IsAny<dm.AffectedProduct>()))
                          .Returns<dm.AffectedProduct>(ap =>
                          {
                              // Capture the inserted value
                              insertedAffectedProduct = ap;
                              ap.affected_product_id = Guid.NewGuid();
                              return ap;
                          });
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var affectedProductIndex = new Mock<IAffectedProductIndex>();
            affectedProductIndex.Setup(ap => ap.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(insertedAffectedProduct);
                           Assert.Equal(insertedAffectedProduct.affected_product_id, id);

                           return insertedAffectedProduct.ToSDKModel();
                       });
            _container.RegisterInstance<IAffectedProductIndex>(affectedProductIndex.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var assignProductOwnerWorker = new TestAssignProductOwnerWorker(_foundation.Object);
            _daemonManager.Setup(dd => dd.GetRegisteredDaemonTask(nameof(AssignProductOwnerWorker)))
                          .Returns(assignProductOwnerWorker);

            var affectedProductController = CreatePost("affectedproducts", _adminAccount);

            var affectedProduct = new sdk.AffectedProduct
            {
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };

            var response = affectedProductController.Create(affectedProduct);

            var httpResponse = HttpAssert.IsCreatedResponse(response);

            var itemResult = HttpAssert.IsContent<ItemResult<sdk.AffectedProduct>>(httpResponse.Content);
            Assert.True(itemResult.success);
            Assert.NotNull(itemResult.item);
            Assert.NotNull(insertedAffectedProduct);
            Assert.Equal(insertedAffectedProduct.affected_product_id, itemResult.item.affected_product_id);
            Assert.Equal(affectedProduct.ticket_id, itemResult.item.ticket_id);
            Assert.Equal(affectedProduct.product_id, itemResult.item.product_id);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _adminAccount.account_id), ticket_id), Times.Once());
        }

        [Fact]
        public void Create_Rejects_NonAdmin_Account_If_Cannot_Update_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var affectedProductController = CreatePost("affectedproducts", _userAccount);

            var affectedProduct = new sdk.AffectedProduct
            {
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };

            var exception = Assert.Throws<HttpResponseException>(() => affectedProductController.Create(affectedProduct));

            _ = HttpAssert.IsUnauthorizedResponse(exception.Response);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
        }

        [Fact]
        public void Create_Kicks_Off_AssignProductOwner_If_No_Assignee()
        {
            Guid ticket_id = Guid.NewGuid();
            dm.AffectedProduct insertedAffectedProduct = null;
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.Insert(It.IsAny<dm.AffectedProduct>()))
                          .Returns<dm.AffectedProduct>(ap =>
                          {
                              // Capture the inserted value
                              insertedAffectedProduct = ap;
                              ap.affected_product_id = Guid.NewGuid();
                              return ap;
                          });
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var affectedProductIndex = new Mock<IAffectedProductIndex>();
            affectedProductIndex.Setup(ap => ap.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(insertedAffectedProduct);
                           Assert.Equal(insertedAffectedProduct.affected_product_id, id);

                           return insertedAffectedProduct.ToSDKModel();
                       });
            _container.RegisterInstance<IAffectedProductIndex>(affectedProductIndex.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(true);
            ticketBusiness.Setup(tt => tt.GetAssignee(ticket_id))
                          .Returns((Guid?)null);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var assignProductOwnerWorker = new TestAssignProductOwnerWorker(_foundation.Object);
            _daemonManager.Setup(dd => dd.GetRegisteredDaemonTask(nameof(AssignProductOwnerWorker)))
                          .Returns(assignProductOwnerWorker);

            var affectedProductController = CreatePost("affectedproducts", _userAccount);

            var affectedProduct = new sdk.AffectedProduct
            {
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };

            var response = affectedProductController.Create(affectedProduct);

            _ = HttpAssert.IsSuccess(response);

            Assert.Collection(
                assignProductOwnerWorker.GetEnqueuedRequests(),
                req =>
                {
                    Assert.Equal(affectedProduct.ticket_id, req.ticket_id);
                    Assert.Equal(affectedProduct.product_id, req.product_id);
                });
        }

        [Fact]
        public void Create_Does_Not_Kick_Off_AssignProductOwner_If_Has_Assignee()
        {
            Guid ticket_id = Guid.NewGuid();
            dm.AffectedProduct insertedAffectedProduct = null;
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.Insert(It.IsAny<dm.AffectedProduct>()))
                          .Returns<dm.AffectedProduct>(ap =>
                          {
                              // Capture the inserted value
                              insertedAffectedProduct = ap;
                              ap.affected_product_id = Guid.NewGuid();
                              return ap;
                          });
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var affectedProductIndex = new Mock<IAffectedProductIndex>();
            affectedProductIndex.Setup(ap => ap.GetById(It.IsAny<Guid>()))
                       .Returns<Guid>(id =>
                       {
                           Assert.NotNull(insertedAffectedProduct);
                           Assert.Equal(insertedAffectedProduct.affected_product_id, id);

                           return insertedAffectedProduct.ToSDKModel();
                       });
            _container.RegisterInstance<IAffectedProductIndex>(affectedProductIndex.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(true);
            ticketBusiness.Setup(tt => tt.GetAssignee(ticket_id))
                          .Returns(_adminAccount.account_id);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var assignProductOwnerWorker = new TestAssignProductOwnerWorker(_foundation.Object);
            _daemonManager.Setup(dd => dd.GetRegisteredDaemonTask(nameof(AssignProductOwnerWorker)))
                          .Returns(assignProductOwnerWorker);

            var affectedProductController = CreatePost("affectedproducts", _userAccount);

            var affectedProduct = new sdk.AffectedProduct
            {
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };

            var response = affectedProductController.Create(affectedProduct);

            _ = HttpAssert.IsSuccess(response);

            Assert.Empty(assignProductOwnerWorker.GetEnqueuedRequests());
        }

        private class TestAssignProductOwnerWorker : AssignProductOwnerWorker
        {
            public TestAssignProductOwnerWorker(IFoundation foundation)
                : base(foundation)
            {
            }

            public IEnumerable<AssignProductOwnerRequest> GetEnqueuedRequests()
                => base.RequestQueue.ToArray();
        }

        [Fact]
        public void Update_Raises_Method_Not_Allowed()
        {
            Guid affected_product_id = Guid.NewGuid();
            var affectedProduct = new sdk.AffectedProduct
            {
                affected_product_id = affected_product_id,
                ticket_id = Guid.NewGuid(),
                product_id = Guid.NewGuid(),
            };

            var affectedProductController = CreatePut($"affectedproducts/{affected_product_id}", _userAccount);

            var exception = Assert.Throws<HttpResponseException>(() => affectedProductController.Update(affected_product_id, affectedProduct));

            var httpResponse = HttpAssert.IsFailure(exception.Response);

            _ = HttpAssert.IsMethodNotAllowed(httpResponse);
        }

        [Fact]
        public void Delete_Allows_Account_If_Can_Update_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();
            Guid affected_product_id = Guid.NewGuid();
            var affectedProduct = new dm.AffectedProduct
            {
                affected_product_id = affected_product_id,
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.GetById(affected_product_id))
                          .Returns<Guid>(id => affectedProduct);
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(true);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var affectedProductController = CreateDelete($"affectedproducts/{affected_product_id}", _userAccount);

            var response = affectedProductController.Delete(affected_product_id);

            var httpResponse = HttpAssert.IsSuccess(response);

            var actionResult = HttpAssert.IsActionResult(httpResponse.Content);
            Assert.True(actionResult.success);
            Assert.NotNull(actionResult.message);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
            affectedProductBusiness.Verify(ap => ap.Delete(affected_product_id), Times.Once());
        }

        [Fact]
        public void Delete_Allows_Admin_Account_If_Cannot_Update_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();
            Guid affected_product_id = Guid.NewGuid();
            var affectedProduct = new dm.AffectedProduct
            {
                affected_product_id = affected_product_id,
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.GetById(affected_product_id))
                          .Returns<Guid>(id => affectedProduct);
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var affectedProductController = CreateDelete($"affectedproducts/{affected_product_id}", _adminAccount);

            var response = affectedProductController.Delete(affected_product_id);

            var httpResponse = HttpAssert.IsSuccess(response);

            var actionResult = HttpAssert.IsActionResult(httpResponse.Content);
            Assert.True(actionResult.success);
            Assert.NotNull(actionResult.message);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _adminAccount.account_id), ticket_id), Times.Once());
            affectedProductBusiness.Verify(ap => ap.Delete(affected_product_id), Times.Once());
        }

        [Fact]
        public void Delete_Rejects_NonAdmin_Account_If_Cannot_Update_Ticket()
        {
            Guid ticket_id = Guid.NewGuid();
            Guid affected_product_id = Guid.NewGuid();
            var affectedProduct = new dm.AffectedProduct
            {
                affected_product_id = affected_product_id,
                ticket_id = ticket_id,
                product_id = Guid.NewGuid(),
            };
            var affectedProductBusiness = new Mock<IAffectedProductBusiness>();
            affectedProductBusiness.Setup(ap => ap.GetById(affected_product_id))
                          .Returns<Guid>(id => affectedProduct);
            _container.RegisterInstance<IAffectedProductBusiness>(affectedProductBusiness.Object);
            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.CanAccountUpdateTicket(It.IsAny<dm.Account>(), ticket_id))
                          .Returns(false);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var affectedProductController = CreateDelete($"affectedproducts/{affected_product_id}", _userAccount);

            var exception = Assert.Throws<HttpResponseException>(() => affectedProductController.Delete(affected_product_id));

            _ = HttpAssert.IsUnauthorizedResponse(exception.Response);

            ticketBusiness.Verify(tt => tt.CanAccountUpdateTicket(It.Is<dm.Account>(aa => aa.account_id == _userAccount.account_id), ticket_id), Times.Once());
        }
    }
}
