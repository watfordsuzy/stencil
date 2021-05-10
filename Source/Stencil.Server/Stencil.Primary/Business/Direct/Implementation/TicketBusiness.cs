using Stencil.Data.Sql;
using System;
using System.Linq;
using dm = Stencil.Domain;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public partial class TicketBusiness : ITicketBusiness
    {
        /// <inheritdoc/>
        public bool CanAccountUpdateTicket(dm.Account account, Guid ticket_id)
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
                    dbTicket ticket = db.dbTickets.SingleOrDefault(tt => tt.ticket_id == ticket_id);
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
        public bool CanAccountDeleteTicket(dm.Account account, Guid ticket_id)
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
                    dbTicket ticket = db.dbTickets.SingleOrDefault(tt => tt.ticket_id == ticket_id);
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
    }
}
