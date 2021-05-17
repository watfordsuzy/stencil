using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sdk = Stencil.SDK.Models;

namespace Stencil.Primary.Business.Index.Implementation
{
    public partial class TicketIndex
    {
        /// <inheritdoc/>
        public void UpdateAssignedTo(Guid ticket_id, Guid? assigned_to_id)
        {
            base.ExecuteMethod(nameof(UpdateAssignedTo), delegate ()
            {
                this.UpdateDocumentPartial(ticket_id.ToString(), new
                {
                    assigned_to_id = assigned_to_id,
                });
            });
        }

        public void UpdateTicketStatus(Guid ticket_id, sdk.TicketStatus ticket_status)
        {
            base.ExecuteMethod(nameof(UpdateTicketStatus), delegate ()
            {
                this.UpdateDocumentPartial(ticket_id.ToString(), new
                {
                    ticket_status = ticket_status,
                });
            });
        }
    }
}
