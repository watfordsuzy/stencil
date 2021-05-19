using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Primary.Synchronization.Implementation
{
    public partial class TicketCommentSynchronizer
    {
        partial void HydrateSDKModel(dm.TicketComment domainModel, sdk.TicketComment sdkModel)
        {
            base.ExecuteMethod(nameof(HydrateSDKModel), delegate ()
            {
                sdk.Account referenceAccount = this.API.Index.Accounts.GetById(sdkModel.commenter_id);
                if (referenceAccount != null)
                {
                    sdkModel.account_name = $"{referenceAccount.first_name} {referenceAccount.last_name}";
                    sdkModel.account_email = referenceAccount.email;
                }
                else
                {
                    dm.Account referenceDomainAccount = this.API.Direct.Accounts.GetById(sdkModel.commenter_id);
                    if (referenceDomainAccount != null)
                    {
                        sdkModel.account_name = $"{referenceDomainAccount.first_name} {referenceDomainAccount.last_name}";
                        sdkModel.account_email = referenceDomainAccount.email;
                    }
                }
            });
        }
    }
}
