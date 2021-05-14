using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stencil.SDK.Models;
using Stencil.Web.Security;
using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public partial class TicketController
    {
        partial void BeforeInsert(Ticket ticket)
        {
            dm.Account account = this.GetCurrentAccount();
            ticket.reported_by_id = account.account_id;
        }

        partial void BeforeUpdate(sdk.Ticket insert)
        {
            base.ExecuteMethod(nameof(BeforeUpdate), delegate ()
            {
                dm.Account account = this.GetCurrentAccount();
                if (!this.API.Direct.Tickets.CanAccountUpdateTicket(account, insert.ticket_id))
                {
                    // If the user cannot inherently update the account, require them to be an admin
                    this.ValidateAdmin();
                }
            });
        }

        partial void BeforeDelete(dm.Ticket delete)
        {
            base.ExecuteMethod(nameof(BeforeDelete), delegate ()
            {
                this.ValidateAdmin();
            });
        }
    }
}