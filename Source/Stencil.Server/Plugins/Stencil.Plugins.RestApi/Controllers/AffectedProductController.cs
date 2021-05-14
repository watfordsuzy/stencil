using Stencil.Primary.Workers;
using Stencil.Primary.Workers.Models;
using Stencil.Web.Security;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public partial class AffectedProductController
    {
        partial void BeforeInsert(sdk.AffectedProduct affectedproduct)
        {
            base.ExecuteMethod(nameof(BeforeInsert), delegate ()
            {
                dm.Account account = this.GetCurrentAccount();
                if (!this.API.Direct.Tickets.CanAccountUpdateTicket(account, affectedproduct.ticket_id))
                {
                    // If the user cannot inherently update the ticket, require them to be an admin
                    this.ValidateAdmin();
                }
            });
        }

        partial void AfterInsert(sdk.AffectedProduct affectedproduct, dm.AffectedProduct inserted)
        {
            base.ExecuteMethod(nameof(AfterInsert), delegate ()
            {
                Guid? assigned_to = this.API.Direct.Tickets.GetAssignee(inserted.ticket_id);
                if (assigned_to == null || assigned_to == Guid.Empty)
                {
                    AssignProductOwnerWorker.EnqueueRequest(
                        this.Foundation,
                        new AssignProductOwnerRequest
                        {
                            ticket_id = inserted.ticket_id,
                            product_id = inserted.product_id,
                        });
                }
            });
        }

        partial void BeforeUpdate(sdk.AffectedProduct affectedproduct)
        {
            base.ExecuteMethod(nameof(BeforeUpdate), (Action)delegate ()
            {
                string message = "Method not allowed";
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
                {
                    Content = new StringContent(message),
                    ReasonPhrase = message,
                });
            });
        }

        partial void BeforeDelete(dm.AffectedProduct delete)
        {
            base.ExecuteMethod(nameof(BeforeDelete), delegate ()
            {
                dm.Account account = this.GetCurrentAccount();
                if (!this.API.Direct.Tickets.CanAccountUpdateTicket(account, delete.ticket_id))
                {
                    // If the user cannot inherently update the ticket, require them to be an admin
                    this.ValidateAdmin();
                }
            });
        }
    }
}