using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Primary.Synchronization.Implementation
{
    public partial class TicketSynchronizer
    {
        partial void HydrateSDKModel(dm.Ticket domainModel, sdk.Ticket sdkModel)
        {
            base.ExecuteMethod(nameof(HydrateSDKModel), delegate ()
            {
                IEnumerable<dm.Product> affectedProducts = this.API.Direct.Products.GetAffectedProductsByTicketID(domainModel.ticket_id);

                sdkModel.affected_products = affectedProducts.Select(pp => pp.product_name).ToArray();
            });
        }
    }
}
