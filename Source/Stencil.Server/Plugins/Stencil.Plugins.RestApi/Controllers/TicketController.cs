using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stencil.Web.Security;
using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public partial class TicketController
    {
        partial void BeforeUpdate(sdk.Ticket insert)
        {
            base.ExecuteMethod(nameof(BeforeUpdate), delegate ()
            {
                this.ValidateAdmin();
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