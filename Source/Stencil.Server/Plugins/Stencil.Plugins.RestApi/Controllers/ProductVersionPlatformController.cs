using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stencil.Web.Security;
using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public partial class ProductVersionPlatformController
    {
        partial void BeforeInsert(sdk.ProductVersionPlatform insert)
        {
            base.ExecuteMethod(nameof(BeforeInsert), delegate ()
            {
                this.ValidateAdmin();
            });
        }

        partial void BeforeUpdate(sdk.ProductVersionPlatform insert)
        {
            base.ExecuteMethod(nameof(BeforeUpdate), delegate ()
            {
                this.ValidateAdmin();
            });
        }

        partial void BeforeDelete(dm.ProductVersionPlatform delete)
        {
            base.ExecuteMethod(nameof(BeforeDelete), delegate ()
            {
                this.ValidateAdmin();
            });
        }
    }
}