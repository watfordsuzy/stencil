using Stencil.Primary.Workers;
using Stencil.Primary.Workers.Models;
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
        partial void BeforeUpdate(sdk.AffectedProduct affectedproduct)
        {
            string message = "Method not allowed";
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
            {
                Content = new StringContent(message),
                ReasonPhrase = message,
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
    }
}