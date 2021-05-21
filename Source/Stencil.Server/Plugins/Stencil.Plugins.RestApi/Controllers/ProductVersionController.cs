using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stencil.Web.Security;
using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public partial class ProductVersionController
    {
        partial void BeforeInsert(sdk.ProductVersion insert)
        {
            base.ExecuteMethod(nameof(BeforeInsert), delegate ()
            {
                this.ValidateAdmin();
            });
        }

        partial void BeforeUpdate(sdk.ProductVersion insert)
        {
            base.ExecuteMethod(nameof(BeforeUpdate), delegate ()
            {
                this.ValidateAdmin();
            });
        }

        partial void BeforeDelete(dm.ProductVersion delete)
        {
            base.ExecuteMethod(nameof(BeforeDelete), delegate ()
            {
                this.ValidateAdmin();
            });
        }
    }
}