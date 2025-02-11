//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#if WINDOWS_PHONE_APP
using RestSharp.Portable;
#else
using RestSharp;
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stencil.SDK.Models;

namespace Stencil.SDK.Endpoints
{
    public partial class TicketCommentEndpoint : EndpointBase
    {
        public TicketCommentEndpoint(StencilSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<TicketComment>> GetTicketCommentAsync(Guid ticket_comment_id)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "ticketcomments/{ticket_comment_id}";
            request.AddUrlSegment("ticket_comment_id", ticket_comment_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<TicketComment>>(request);
        }
        
        
        public Task<ListResult<TicketComment>> GetTicketCommentByTicketIDAsync(Guid ticket_id, int skip = 0, int take = 10, string order_by = "", bool descending = false)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "ticketcomments/by_ticketid/{ticket_id}";
            request.AddUrlSegment("ticket_id", ticket_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            
            return this.Sdk.ExecuteAsync<ListResult<TicketComment>>(request);
        }
        
        public Task<ListResult<TicketComment>> GetTicketCommentByCommenterIDAsync(Guid account_id, int skip = 0, int take = 10, string order_by = "", bool descending = false)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "ticketcomments/by_commenterid/{account_id}";
            request.AddUrlSegment("account_id", account_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            
            return this.Sdk.ExecuteAsync<ListResult<TicketComment>>(request);
        }
        

        public Task<ItemResult<TicketComment>> CreateTicketCommentAsync(TicketComment ticketcomment)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "ticketcomments";
            request.AddJsonBody(ticketcomment);
            return this.Sdk.ExecuteAsync<ItemResult<TicketComment>>(request);
        }

        public Task<ItemResult<TicketComment>> UpdateTicketCommentAsync(Guid ticket_comment_id, TicketComment ticketcomment)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "ticketcomments/{ticket_comment_id}";
            request.AddUrlSegment("ticket_comment_id", ticket_comment_id.ToString());
            request.AddJsonBody(ticketcomment);
            return this.Sdk.ExecuteAsync<ItemResult<TicketComment>>(request);
        }

        

        public Task<ActionResult> DeleteTicketCommentAsync(Guid ticket_comment_id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "ticketcomments/{ticket_comment_id}";
            request.AddUrlSegment("ticket_comment_id", ticket_comment_id.ToString());
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
