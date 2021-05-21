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

        public static IEnumerable<object[]> Insert_Sets_Specific_Properties_TestData()
        {
            foreach (var ticketStatus in Enum.GetValues(typeof(dm.TicketStatus)))
            {
                foreach (var ticketType in Enum.GetValues(typeof(dm.TicketType)))
                {
                    yield return new object[] { ticketStatus, ticketType, };
                }
            }
        }

        [Theory]
        [MemberData(nameof(Insert_Sets_Specific_Properties_TestData))]
        public void Insert_Sets_Specific_Properties(dm.TicketStatus ticketStatus, dm.TicketType ticketType)
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new Mock<ITicketSynchronizer>();
            _container.RegisterInstance<ITicketSynchronizer>(ticketSynchronizer.Object);

            DateTime now = DateTime.UtcNow;

            // When inserting a new ticket, the following properties should be overridden:
            //    - ticket_status => TicketStatus.Open
            //    - opened_on_utc => DateTime.UtcNow
            //    - closed_on_utc => null
            //
            // The rest should be left as-is.
            var ticket = new dm.Ticket
            {
                ticket_title = "Test",
                ticket_description = "Description",
                closed_on_utc = now.Subtract(TimeSpan.FromDays(10)),
                opened_on_utc = now.Subtract(TimeSpan.FromDays(100)),
                ticket_status = ticketStatus,
                ticket_type = ticketType,
                reported_by_id = _reportedByAccount.account_id,
                assigned_to_id = _assignedToAccount.account_id,
            };

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            dm.Ticket insertedTicket = ticketBusiness.Insert(ticket);
            Assert.NotEqual(Guid.Empty, insertedTicket.ticket_id);

            Assert.Equal(ticket.ticket_title, insertedTicket.ticket_title);
            Assert.Equal(ticket.ticket_description, insertedTicket.ticket_description);
            Assert.Null(insertedTicket.closed_on_utc);
            Assert.True(insertedTicket.opened_on_utc >= now);
            Assert.Equal(dm.TicketStatus.Open, insertedTicket.ticket_status);
            Assert.Equal(ticket.ticket_type, insertedTicket.ticket_type);
            Assert.Equal(ticket.reported_by_id, insertedTicket.reported_by_id);
            Assert.Equal(ticket.assigned_to_id, insertedTicket.assigned_to_id);
        }

        [Fact]
        public void Update_To_Closed_Sets_ClosedOnDate()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new Mock<ITicketSynchronizer>();
            _container.RegisterInstance<ITicketSynchronizer>(ticketSynchronizer.Object);

            DateTime now = DateTime.UtcNow;

            // When we close this ticket its closed on date should be after `now`
            var ticket = new dm.Ticket
            {
                ticket_title = "Test",
                ticket_description = "Description",
                ticket_status = dm.TicketStatus.Open,
                ticket_type = dm.TicketType.TechDebt,
                reported_by_id = _reportedByAccount.account_id,
            };

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            // Setup the system to have an Open ticket we can close
            dm.Ticket insertedTicket = ticketBusiness.Insert(ticket);

            // Close the ticket
            insertedTicket.ticket_status = dm.TicketStatus.Closed;

            dm.Ticket updatedTicket = ticketBusiness.Update(insertedTicket);

            Assert.Equal(dm.TicketStatus.Closed, updatedTicket.ticket_status);
            Assert.True(updatedTicket.closed_on_utc >= now);
        }

        [Fact]
        public void Update_If_Already_Closed_Leaves_ClosedOnDate_Alone()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new Mock<ITicketSynchronizer>();
            _container.RegisterInstance<ITicketSynchronizer>(ticketSynchronizer.Object);

            DateTime now = DateTime.UtcNow;
            var ticket = new dm.Ticket
            {
                ticket_title = "Test",
                ticket_description = "Description",
                ticket_status = dm.TicketStatus.Open,
                ticket_type = dm.TicketType.TechDebt,
                reported_by_id = _reportedByAccount.account_id,
            };

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            dm.Ticket insertedTicket = ticketBusiness.Insert(ticket);

            insertedTicket.ticket_status = dm.TicketStatus.Closed;
            
            dm.Ticket closedTicket = ticketBusiness.Update(insertedTicket);
            Assert.NotNull(closedTicket.closed_on_utc);

            DateTime originalClosedOn = closedTicket.closed_on_utc.Value;

            // Update the closed on date to something else as well as
            // updating a different property.
            closedTicket.closed_on_utc = DateTime.UtcNow.AddDays(30);
            closedTicket.assigned_to_id = _assignedToAccount.account_id;
            dm.Ticket updatedTicket = ticketBusiness.Update(closedTicket);

            // The closed on date should not have changed
            Assert.Equal(originalClosedOn, updatedTicket.closed_on_utc);
            Assert.Equal(closedTicket.assigned_to_id, updatedTicket.assigned_to_id);
        }

        [Fact]
        public void Update_To_Reopened_Clears_ClosedOnDate()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new Mock<ITicketSynchronizer>();
            _container.RegisterInstance<ITicketSynchronizer>(ticketSynchronizer.Object);

            DateTime now = DateTime.UtcNow;
            var ticket = new dm.Ticket
            {
                ticket_title = "Test",
                ticket_description = "Description",
                ticket_status = dm.TicketStatus.Open,
                ticket_type = dm.TicketType.TechDebt,
                reported_by_id = _reportedByAccount.account_id,
            };

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            dm.Ticket insertedTicket = ticketBusiness.Insert(ticket);

            insertedTicket.ticket_status = dm.TicketStatus.Closed;

            dm.Ticket closedTicket = ticketBusiness.Update(insertedTicket);
            Assert.NotNull(closedTicket.closed_on_utc);

            // Re-open the ticket
            closedTicket.ticket_status = dm.TicketStatus.Open;
            closedTicket.assigned_to_id = _assignedToAccount.account_id;
            dm.Ticket reopenedTicket = ticketBusiness.Update(closedTicket);

            // The closed on date should not have changed
            Assert.Equal(dm.TicketStatus.Open, reopenedTicket.ticket_status);
            Assert.Null(reopenedTicket.closed_on_utc);
            Assert.Equal(closedTicket.assigned_to_id, reopenedTicket.assigned_to_id);
        }

        [Fact]
        public void MarkTicketAsInProgress_Ignores_Missing_Tickets()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            ticketBusiness.MarkTicketAsInProgress(Guid.Empty, null);

            ticketBusiness.MarkTicketAsInProgress(Guid.Empty, Guid.NewGuid());
            
            ticketBusiness.MarkTicketAsInProgress(Guid.NewGuid(), null);
            
            ticketBusiness.MarkTicketAsInProgress(Guid.NewGuid(), Guid.NewGuid());

            ticketIndex.Verify(tt => tt.UpdateTicketStatus(It.IsAny<Guid>(), It.IsAny<sdk.TicketStatus>()), Times.Never());
        }

        [Fact]
        public void MarkTicketAsInProgress_Ignores_NonOpen_Tickets()
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketSynchronizer = new Mock<ITicketSynchronizer>();
            _container.RegisterInstance<ITicketSynchronizer>(ticketSynchronizer.Object);

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            var insertedTicket = ticketBusiness.Insert(new dm.Ticket
            {
                ticket_title = "Test",
                ticket_description = "Description",
                ticket_status = dm.TicketStatus.Open,
                ticket_type = dm.TicketType.TechDebt,
                reported_by_id = _reportedByAccount.account_id,
            });
            insertedTicket.ticket_status = dm.TicketStatus.Closed;
            var closedTicket = ticketBusiness.Update(insertedTicket);

            insertedTicket = ticketBusiness.Insert(new dm.Ticket
            {
                ticket_title = "Test",
                ticket_description = "Description",
                ticket_status = dm.TicketStatus.Open,
                ticket_type = dm.TicketType.Feature,
                reported_by_id = _reportedByAccount.account_id,
            });
            insertedTicket.ticket_status = dm.TicketStatus.InProgress;
            var inProgressTicket = ticketBusiness.Update(insertedTicket);

            ticketBusiness.MarkTicketAsInProgress(closedTicket.ticket_id, null);

            ticketBusiness.MarkTicketAsInProgress(closedTicket.ticket_id, Guid.NewGuid());

            ticketBusiness.MarkTicketAsInProgress(inProgressTicket.ticket_id, null);

            ticketBusiness.MarkTicketAsInProgress(inProgressTicket.ticket_id, Guid.NewGuid());

            ticketIndex.Verify(tt => tt.UpdateTicketStatus(It.IsAny<Guid>(), It.IsAny<sdk.TicketStatus>()), Times.Never());
        }

        public static IEnumerable<object[]> MarkTicketAsInProgress_Marks_Open_Tickets_As_InProgress_TestData()
        {
            yield return new object[] { Guid.NewGuid() };
            yield return new object[] { (Guid?)null };
        }

        [Theory]
        [MemberData(nameof(MarkTicketAsInProgress_Marks_Open_Tickets_As_InProgress_TestData))]
        public void MarkTicketAsInProgress_Marks_Open_Tickets_As_InProgress(Guid? commit_id)
        {
            var ticketIndex = new Mock<ITicketIndex>();
            _container.RegisterInstance<ITicketIndex>(ticketIndex.Object);

            var ticketBusiness = new TicketBusiness(_foundation.Object);

            ticketBusiness.MarkTicketAsInProgress(_ticket0.ticket_id, commit_id);

            // Idempotent
            ticketBusiness.MarkTicketAsInProgress(_ticket0.ticket_id, commit_id);

            ticketIndex.Verify(tt => tt.UpdateTicketStatus(_ticket0.ticket_id, sdk.TicketStatus.InProgress), Times.Once());
        }
    }
}
