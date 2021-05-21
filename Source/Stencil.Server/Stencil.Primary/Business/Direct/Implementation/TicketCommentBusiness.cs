using Stencil.Data.Sql;
using Stencil.Domain;
using System;
using System.Linq;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public partial class TicketCommentBusiness
    {
        /// <inheritdoc/>
        public bool CanAccountUpdateTicketComment(Account account, Guid ticket_comment_id)
        {
            return base.ExecuteFunction(nameof(CanAccountUpdateTicketComment), delegate ()
            {
                // Do not attempt to lookup empty tickets or accounts
                if (ticket_comment_id == Guid.Empty || account == null || account.account_id == Guid.Empty)
                {
                    return false;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicketComment ticketComment = db.dbTicketComments.FirstOrDefault(tt => tt.ticket_comment_id == ticket_comment_id);
                    if (ticketComment == null)
                    {
                        return false;
                    }

                    // The account which added the ticket comment may update the ticket comment
                    if (ticketComment.commenter_id == account.account_id)
                    {
                        return true;
                    }

                    // No other accounts may inherently update the ticket comment
                    return false;
                }
            });
        }

        /// <inheritdoc/>
        public bool CanAccountDeleteTicketComment(Account account, Guid ticket_comment_id)
        {
            return base.ExecuteFunction(nameof(CanAccountDeleteTicketComment), delegate ()
            {
                // Do not attempt to lookup empty tickets or accounts
                if (ticket_comment_id == Guid.Empty || account == null || account.account_id == Guid.Empty)
                {
                    return false;
                }

                using (var db = base.CreateSQLContext())
                {
                    dbTicketComment ticketComment = db.dbTicketComments.FirstOrDefault(tt => tt.ticket_comment_id == ticket_comment_id);
                    if (ticketComment == null)
                    {
                        return false;
                    }

                    // The account which added the ticket comment may delete the ticket comment
                    if (ticketComment.commenter_id == account.account_id)
                    {
                        return true;
                    }

                    // No other accounts may inherently delete the ticket comment
                    return false;
                }
            });
        }
    }
}
