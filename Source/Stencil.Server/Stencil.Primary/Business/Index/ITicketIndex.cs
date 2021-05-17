using Stencil.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Index
{
    public partial interface ITicketIndex
    {
        /// <summary>
        /// Updates the ticket assignee.
        /// </summary>
        /// <param name="ticket_id">The unique identifier of the ticket.</param>
        /// <param name="assigned_to_id">The unique identifier of the new assignee,
        /// otherwise <see langword="null"/>.</param>
        void UpdateAssignedTo(Guid ticket_id, Guid? assigned_to_id);

        /// <summary>
        /// Updates the status on a ticket.
        /// </summary>
        /// <param name="ticket_id">The unique identifier of the ticket.</param>
        /// <param name="ticket_status">The new status of the ticket.</param>
        void UpdateTicketStatus(Guid ticket_id, TicketStatus ticket_status);
    }
}
