//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using Stencil.SDK.Models;
using Stencil.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Index
{
    public partial interface ITicketCommentIndex : IIndexer<TicketComment>
    {
        TicketComment GetById(Guid id);
        TCustomModel GetById<TCustomModel>(Guid id)
            where TCustomModel : class;
        ListResult<TicketComment> GetByTicketID(Guid ticket_id, int skip, int take, string order_by = "", bool descending = false);
        ListResult<TicketComment> GetByCommenterID(Guid account_id, int skip, int take, string order_by = "", bool descending = false);
        
    }
}
