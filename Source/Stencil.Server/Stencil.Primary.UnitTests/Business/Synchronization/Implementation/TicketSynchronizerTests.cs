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
    public class TicketSynchronizerTests : SynchronizerTestBase
    {
        [Fact]
        public void HydrateSDKModel_Allows_Empty_AffectedProducts()
        {
            Guid ticket_id = Guid.NewGuid();
            dm.Ticket ticket = new dm.Ticket
            {
                ticket_id = ticket_id,
            };

            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.GetById(ticket_id))
                          .Returns(ticket);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);
            var productBusiness = new Mock<IProductBusiness>();
            productBusiness.Setup(pp => pp.GetAffectedProductsByTicketID(ticket_id))
                           .Returns(Enumerable.Empty<dm.Product>());
            _container.RegisterInstance<IProductBusiness>(productBusiness.Object);

            sdk.Ticket updatedTicket = null;
            var ticketIndex = new Mock<ITicketIndex>();
            ticketIndex.Setup(tt => tt.UpdateDocument(It.IsAny<sdk.Ticket>()))
                       .Callback<sdk.Ticket>(tt =>
                       {
                           updatedTicket = tt;
                       })
                       .Returns(new IndexResult { success = true });
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new TicketSynchronizer(_foundation.Object);

            ticketSynchronizer.PerformSynchronizationForItem(ticket_id);

            Assert.Equal(ticket_id, updatedTicket.ticket_id);
            Assert.Empty(updatedTicket.affected_products);
        }

        [Fact]
        public void HydrateSDKModel_Includes_AffectedProducts()
        {
            Guid ticket_id = Guid.NewGuid();
            dm.Ticket ticket = new dm.Ticket
            {
                ticket_id = ticket_id,
            };
            var products = new[]
            {
                new dm.Product
                {
                    product_id = Guid.NewGuid(),
                    product_name = "Product 0",
                },
                new dm.Product
                {
                    product_id = Guid.NewGuid(),
                    product_name = "Product 1",
                },
            };

            var ticketBusiness = new Mock<ITicketBusiness>();
            ticketBusiness.Setup(tt => tt.GetById(ticket_id))
                          .Returns(ticket);
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);
            var productBusiness = new Mock<IProductBusiness>();
            productBusiness.Setup(pp => pp.GetAffectedProductsByTicketID(ticket_id))
                           .Returns(products);
            _container.RegisterInstance<IProductBusiness>(productBusiness.Object);

            sdk.Ticket updatedTicket = null;
            var ticketIndex = new Mock<ITicketIndex>();
            ticketIndex.Setup(tt => tt.UpdateDocument(It.IsAny<sdk.Ticket>()))
                       .Callback<sdk.Ticket>(tt =>
                       {
                           updatedTicket = tt;
                       })
                       .Returns(new IndexResult { success = true });
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new TicketSynchronizer(_foundation.Object);

            ticketSynchronizer.PerformSynchronizationForItem(ticket_id);

            Assert.Equal(ticket_id, updatedTicket.ticket_id);
            Assert.Collection(
                updatedTicket.affected_products,
                p0 => Assert.Equal(products[0].product_name, p0),
                p1 => Assert.Equal(products[1].product_name, p1));
        }
    }
}
