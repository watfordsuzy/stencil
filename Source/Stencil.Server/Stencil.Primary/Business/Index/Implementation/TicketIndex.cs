using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
