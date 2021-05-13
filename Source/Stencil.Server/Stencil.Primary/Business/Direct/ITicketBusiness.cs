using Stencil.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Direct
{
    public partial interface ITicketBusiness
    {
        /// <summary>
        /// Determines if a given account has access to update a ticket.
        /// </summary>
        /// <param name="account">The given user account.</param>
        /// <param name="ticket_id">The unique identifier of the ticket.</param>
        /// <returns><see langword="true"/> if and only if the account has access
        /// to update the ticket; otherwise <see langword="false"/>.</returns>
        bool CanAccountUpdateTicket(Account account, Guid ticket_id);

        /// <summary>
        /// Determines if a given account has access to delete a ticket.
        /// </summary>
        /// <param name="account">The given user account.</param>
        /// <param name="ticket_id">The unique identifier of the ticket.</param>
        /// <returns><see langword="true"/> if and only if the account has access
        /// to delete the ticket; otherwise <see langword="false"/>.</returns>
        bool CanAccountDeleteTicket(Account account, Guid ticket_id);

        /// <summary>
        /// Gets the unique identifier of the account assigned to work
        /// on a ticket, if one exists.
        /// </summary>
        /// <param name="ticket_id">The unique identifier of the ticket.</param>
        /// <returns>The unique identifier of the account assigned to work
        /// on the ticket, otherwise <see langword="null"/>.</returns>
        Guid? GetAssignee(Guid ticket_id);

        /// <summary>
        /// Assigns a ticket to the owner of a product.
        /// </summary>
        /// <param name="ticket_id">The unique identifier of the ticket.</param>
        /// <param name="product_id">The unique identifier of the product.</param>
        void AssignToProductOwner(Guid ticket_id, Guid product_id);
    }
}
