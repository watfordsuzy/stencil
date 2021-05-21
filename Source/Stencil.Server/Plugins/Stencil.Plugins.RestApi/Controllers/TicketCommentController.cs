using Stencil.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public partial class TicketCommentController
    {
        partial void BeforeInsert(sdk.TicketComment ticketComment)
        {
            base.ExecuteMethod(nameof(BeforeInsert), delegate ()
            {
                dm.Account account = this.GetCurrentAccount();
                ticketComment.commenter_id = account.account_id;
                ticketComment.commented_on_utc = DateTime.UtcNow;
            });
        }

        partial void BeforeUpdate(sdk.TicketComment insert)
        {
            base.ExecuteMethod(nameof(BeforeUpdate), delegate ()
            {
                dm.Account account = this.GetCurrentAccount();
                if (!this.API.Direct.TicketComments.CanAccountUpdateTicketComment(account, insert.ticket_comment_id))
                {
                    // If the user cannot inherently update the ticket comment, require them to be an admin
                    this.ValidateAdmin();
                }
            });
        }

        partial void BeforeDelete(dm.TicketComment delete)
        {
            base.ExecuteMethod(nameof(BeforeDelete), delegate ()
            {
                dm.Account account = this.GetCurrentAccount();
                if (!this.API.Direct.TicketComments.CanAccountDeleteTicketComment(account, delete.ticket_comment_id))
                {
                    // If the user cannot inherently delete the ticket comment, require them to be an admin
                    this.ValidateAdmin();
                }
            });
        }
    }
}