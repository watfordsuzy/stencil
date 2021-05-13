using Stencil.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using dm = Stencil.Domain;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public class ProductBusinesTests : BusinessTestBase
    {
        private readonly Guid _account;
        private readonly Guid _ticketWithoutAffectedProducts;
        private readonly Guid _ticketWithAffectedProducts;
        private readonly dm.Product _product0;
        private readonly dm.Product _product1;

        public ProductBusinesTests()
            : base()
        {
            _account = Guid.NewGuid();
            _ticketWithoutAffectedProducts = Guid.NewGuid();
            _ticketWithAffectedProducts = Guid.NewGuid();
            _product0 = new dm.Product
            {
                product_id = Guid.NewGuid(),
                product_name = "Product 0",
            };
            _product1 = new dm.Product
            {
                product_id = Guid.NewGuid(),
                product_name = "Product 1",
            };

            _context.dbAccounts.Add(new dbAccount
            {
                account_id = _account,
                email = "test0@example.com",
                password = "password",
                password_salt = "veruca",
                api_key = "key",
                api_secret = "secret",
            });
            _context.dbTickets.Add(new dbTicket
            {
                ticket_id = _ticketWithoutAffectedProducts,
                reported_by_id = _account,
                assigned_to_id = _account,
                ticket_title = "Title",
                ticket_description = "Description",
                opened_on_utc = DateTimeOffset.UtcNow,
                ticket_type = (int)dm.TicketType.Bug,
                ticket_status = (int)dm.TicketStatus.Open,
            });
            _context.dbTickets.Add(new dbTicket
            {
                ticket_id = _ticketWithAffectedProducts,
                reported_by_id = _account,
                assigned_to_id = _account,
                ticket_title = "Title",
                ticket_description = "Description",
                opened_on_utc = DateTimeOffset.UtcNow,
                ticket_type = (int)dm.TicketType.Bug,
                ticket_status = (int)dm.TicketStatus.InProgress,
            });
            _context.dbProducts.Add(new dbProduct
            {
                product_id = _product0.product_id,
                product_name = _product0.product_name,
                product_owner_id = _account,
            });
            _context.dbProducts.Add(new dbProduct
            {
                product_id = _product1.product_id,
                product_name = _product1.product_name,
                product_owner_id = _account,
            });
            _context.dbAffectedProducts.Add(new dbAffectedProduct
            {
                affected_product_id = Guid.NewGuid(),
                ticket_id = _ticketWithAffectedProducts,
                product_id = _product0.product_id,
            });
            _context.dbAffectedProducts.Add(new dbAffectedProduct
            {
                affected_product_id = Guid.NewGuid(),
                ticket_id = _ticketWithAffectedProducts,
                product_id = _product1.product_id,
            });
            _context.SaveChanges();
        }

        [Fact]
        public void GetAffectedProductsbyTicketID_Returns_Empty_If_No_Ticket()
        {
            var productBusiness = new ProductBusiness(_foundation.Object);

            Assert.Empty(productBusiness.GetAffectedProductsByTicketID(Guid.Empty));
        }

        [Fact]
        public void GetAffectedProductsbyTicketID_Returns_Empty_If_Ticket_Has_No_AffectedProducts()
        {
            var productBusiness = new ProductBusiness(_foundation.Object);

            Assert.Empty(productBusiness.GetAffectedProductsByTicketID(_ticketWithoutAffectedProducts));
        }

        [Fact]
        public void GetAffectedProductsbyTicketID_Returns_AffectedProducts()
        {
            var productBusiness = new ProductBusiness(_foundation.Object);

            IEnumerable<dm.Product> products = productBusiness.GetAffectedProductsByTicketID(_ticketWithAffectedProducts);
            Assert.Collection(
                products.OrderBy(pp => pp.product_name),
                p0 =>
                {
                    Assert.Equal(_product0.product_id, p0.product_id);
                    Assert.Equal(_product0.product_name, p0.product_name);
                },
                p1 =>
                {
                    Assert.Equal(_product1.product_id, p1.product_id);
                    Assert.Equal(_product1.product_name, p1.product_name);
                });
        }
    }
}
