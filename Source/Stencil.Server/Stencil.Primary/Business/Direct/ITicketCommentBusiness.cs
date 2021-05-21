using Stencil.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Direct
{
    public partial interface ITicketCommentBusiness
    {
        /// <summary>
        /// Determines if a given account has access to update a ticket comment.
        /// </summary>
        /// <param name="account">The given user account.</param>
        /// <param name="ticket_comment_id">The unique identifier of the ticket comment.</param>
        /// <returns><see langword="true"/> if and only if the account has access
        /// to update the ticket comment; otherwise <see langword="false"/>.</returns>
        bool CanAccountUpdateTicketComment(Account account, Guid ticket_comment_id);

        /// <summary>
        /// Determines if a given account has access to delete a ticket comment.
        /// </summary>
        /// <param name="account">The given user account.</param>
        /// <param name="ticket_id">The unique identifier of the ticket comment.</param>
        /// <returns><see langword="true"/> if and only if the account has access
        /// to delete the ticket comment; otherwise <see langword="false"/>.</returns>
        bool CanAccountDeleteTicketComment(Account account, Guid ticket_comment_id);
    }
}
