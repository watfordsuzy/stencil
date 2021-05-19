using Stencil.Data.Sql;
using Stencil.Domain;
using System;
using System.Linq;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public partial class TicketBusiness : ITicketBusiness
    {
        partial void PreProcess(Ticket ticket, bool forInsert)
        {
            base.ExecuteMethod(nameof(PreProcess), delegate ()
            {
                if (forInsert)
                {
                    // Brand new tickets have the following properties automatically set,
                    // because it does not make sense for a user to update these.
                    ticket.opened_on_utc = DateTime.UtcNow;
                    ticket.closed_on_utc = null;
                    ticket.ticket_status = TicketStatus.Open;
                }
                else
                {
                    if (ticket.ticket_status != TicketStatus.Closed)
                    {
                        // Ensure open tickets do not have a closed on date set during updates.
                        ticket.closed_on_utc = null;
                    }
                }
            });
        }

        partial void BeforeUpdatePersisted(dbTicket ticket, Ticket previous)
        {
            base.ExecuteMethod(nameof(BeforeUpdatePersisted), delegate ()
            {
                // Ensure we do not reset the closed on date
                if (previous.ticket_status == TicketStatus.Closed
                 && ticket.ticket_status == (int)TicketStatus.Closed
                 && ticket.closed_on_utc != previous.closed_on_utc)
                {
                    ticket.closed_on_utc = previous.closed_on_utc;
                }

                // Set the closed on date for tickets that were just closed
                if (previous.ticket_status != TicketStatus.Closed
                 && ticket.ticket_status == (int)TicketStatus.Closed)
                {
                    ticket.closed_on_utc = DateTime.UtcNow;
                }
            });
        }

        /// <inheritdoc/>
        public bool CanAccountUpdateTicket(Account account, Guid ticket_id)
        {
            return base.ExecuteFunction(nameof(CanAccountUpdateTicket), delegate ()
            {
                // Do not attempt to lookup empty tickets or accounts
                if (ticket_id == Guid.Empty || account == null || account.account_id == Guid.Empty)
                {
                    return false;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicket ticket = db.dbTickets.FirstOrDefault(tt => tt.ticket_id == ticket_id);
                    if (ticket == null)
                    {
                        return false;
                    }

                    // The account which opened the ticket may update the ticket
                    if (ticket.reported_by_id == account.account_id)
                    {
                        return true;
                    }

                    // The account the ticket is assigned to may update the ticket
                    if (ticket.assigned_to_id == account.account_id)
                    {
                        return true;
                    }

                    // No other accounts may inherently update the ticket
                    return false;
                }
            });
        }

        /// <inheritdoc/>
        public bool CanAccountDeleteTicket(Account account, Guid ticket_id)
        {
            return base.ExecuteFunction(nameof(CanAccountDeleteTicket), delegate ()
            {
                // Do not attempt to lookup empty tickets or accounts
                if (ticket_id == Guid.Empty || account == null || account.account_id == Guid.Empty)
                {
                    return false;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicket ticket = db.dbTickets.FirstOrDefault(tt => tt.ticket_id == ticket_id);
                    if (ticket == null)
                    {
                        return false;
                    }

                    // The account the ticket is assigned to may delete the ticket
                    if (ticket.assigned_to_id == account.account_id)
                    {
                        return true;
                    }

                    // No other accounts may inherently update the ticket
                    return false;
                }
            });
        }

        /// <inheritdoc/>
        public Guid? GetAssignee(Guid ticket_id)
        {
            return base.ExecuteFunction(nameof(GetAssignee), delegate ()
            {
                // Do not attempt to lookup empty tickets
                if (ticket_id == Guid.Empty)
                {
                    return null;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicket ticket = db.dbTickets.FirstOrDefault(tt => tt.ticket_id == ticket_id);
                    return ticket?.assigned_to_id;
                }
            });
        }

        /// <inheritdoc/>
        public void AssignToProductOwner(Guid ticket_id, Guid product_id)
        {
            base.ExecuteMethod(nameof(AssignToProductOwner), delegate ()
            {
                // Do not attempt to lookup empty tickets or accounts
                if (ticket_id == Guid.Empty || product_id == Guid.Empty)
                {
                    return;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicket ticket = db.dbTickets.FirstOrDefault(tt => tt.ticket_id == ticket_id);
                    Guid? product_owner_id = db.dbProducts.FirstOrDefault(pp => pp.product_id == product_id)?.product_owner_id;
                    if (ticket != null && product_owner_id != null && product_owner_id != Guid.Empty)
                    {
                        ticket.assigned_to_id = product_owner_id;

                        db.SaveChanges();

                        this.API.Index.Tickets.UpdateAssignedTo(ticket_id, ticket.ToDomainModel().assigned_to_id);
                    }
                }
            });
        }

        public void MarkTicketAsInProgress(Guid ticket_id, Guid? commit_id)
        {
            base.ExecuteMethod(nameof(MarkTicketAsInProgress), delegate ()
            {
                // Do not attempt to lookup empty tickets
                if (ticket_id == Guid.Empty)
                {
                    return;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicket ticket = db.dbTickets.FirstOrDefault(tt => tt.ticket_id == ticket_id);
                    if (ticket != null && ticket.ticket_status == (int)TicketStatus.Open)
                    {
                        ticket.ticket_status = (int)TicketStatus.InProgress;

                        db.SaveChanges();

                        // CAW: this seems off, is there a better way?
                        this.API.Index.Tickets.UpdateTicketStatus(
                            ticket_id,
                            ticket.ToDomainModel().ToSDKModel().ticket_status);
                    }
                }
            });
        }
    }
}
