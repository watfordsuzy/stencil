//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using Codeable.Foundation.Core;
using System;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using sdk = Stencil.SDK.Models;
using dm = Stencil.Domain;
using Stencil.Primary;
using Stencil.SDK;
using Stencil.Web.Controllers;
using Stencil.Web.Security;

namespace Stencil.Plugins.RestAPI.Controllers
{
    [ApiKeyHttpAuthorize]
    [RoutePrefix("api/tickets")]
    public partial class TicketController : HealthRestApiController
    {
        public TicketController(IFoundation foundation)
            : base(foundation, "Ticket")
        {
        }

        [HttpGet]
        [Route("{ticket_id}")]
        public object GetById(Guid ticket_id)
        {
            return base.ExecuteFunction<object>("GetById", delegate()
            {
                this.BeforeGet();

                sdk.Ticket result = this.API.Index.Tickets.GetById(ticket_id);
                if (result == null)
                {
                    return Http404("Ticket");
                }

                this.AfterGet(result);

                

                return base.Http200(new ItemResult<sdk.Ticket>()
                {
                    success = true, 
                    item = result
                });
            });
        }

        partial void BeforeGet();
        partial void AfterGet(sdk.Ticket result);
        partial void AfterGet(ListResult<sdk.Ticket> result);
        
        
        [HttpGet]
        [Route("")]
        public object Find(int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "", Guid? reported_by_id = null, Guid? assigned_to_id = null)
        {
            return base.ExecuteFunction<object>("Find", delegate()
            {
                this.BeforeFind();

                
                ListResult<sdk.Ticket> result = this.API.Index.Tickets.Find(skip, take, keyword, order_by, descending, reported_by_id, assigned_to_id);
                result.success = true;

                this.AfterFind(result);

                return base.Http200(result);
            });
        }
        
        partial void BeforeFind();
        partial void AfterFind(ListResult<sdk.Ticket> result);
        
        
        [HttpGet]
        [Route("by_reportedbyid/{account_id}")]
        public object GetByReportedByID(Guid account_id, int skip = 0, int take = 10, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction<object>("GetByReportedByID", delegate ()
            {
                this.BeforeGet();

                
                ListResult<sdk.Ticket> result = this.API.Index.Tickets.GetByReportedByID(account_id, skip, take, order_by, descending);
                result.success = true;

                this.AfterGet(result);

                return base.Http200(result);
            });
        }
        
        [HttpGet]
        [Route("by_assignedtoid/{account_id}")]
        public object GetByAssignedToID(Guid account_id, int skip = 0, int take = 10, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction<object>("GetByAssignedToID", delegate ()
            {
                this.BeforeGet();

                
                ListResult<sdk.Ticket> result = this.API.Index.Tickets.GetByAssignedToID(account_id, skip, take, order_by, descending);
                result.success = true;

                this.AfterGet(result);

                return base.Http200(result);
            });
        }
        
        
        
       

        [HttpPost]
        [Route("")]
        public object Create(sdk.Ticket ticket)
        {
            return base.ExecuteFunction<object>("Create", delegate()
            {
                this.ValidateNotNull(ticket, "Ticket");

                this.BeforeInsert(ticket);

                dm.Ticket insert = ticket.ToDomainModel();
              
                insert = this.API.Direct.Tickets.Insert(insert);
                
                this.AfterInsert(ticket, insert);

                
                sdk.Ticket result = this.API.Index.Tickets.GetById(insert.ticket_id);

                return base.Http201(new ItemResult<sdk.Ticket>()
                {
                    success = true,
                    item = result
                }
                , string.Format("api/ticket/{0}", ticket.ticket_id));

            });

        }

        partial void BeforeInsert(sdk.Ticket ticket);
        partial void AfterInsert(sdk.Ticket ticket, dm.Ticket inserted);

        [HttpPut]
        [Route("{ticket_id}")]
        public object Update(Guid ticket_id, sdk.Ticket ticket)
        {
            return base.ExecuteFunction<object>("Update", delegate()
            {
                this.ValidateNotNull(ticket, "Ticket");
                this.ValidateRouteMatch(ticket_id, ticket.ticket_id, "Ticket");

                ticket.ticket_id = ticket_id;

                this.BeforeUpdate(ticket);

                dm.Ticket update = ticket.ToDomainModel();

                update = this.API.Direct.Tickets.Update(update);

                this.AfterUpdate(ticket, update);
                
                
                sdk.Ticket existing = this.API.Index.Tickets.GetById(update.ticket_id);
                
                
                return base.Http200(new ItemResult<sdk.Ticket>()
                {
                    success = true,
                    item = existing
                });

            });

        }

        partial void BeforeUpdate(sdk.Ticket ticket);
        partial void AfterUpdate(sdk.Ticket ticket, dm.Ticket updated);

        

        [HttpDelete]
        [Route("{ticket_id}")]
        public object Delete(Guid ticket_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Ticket delete = this.API.Direct.Tickets.GetById(ticket_id);
                if (delete == null)
                {
                    return base.Http404(@"Ticket");
                }
                
                this.BeforeDelete(delete);
                
                this.API.Direct.Tickets.Delete(ticket_id);

                return base.Http200(new ActionResult()
                {
                    success = true,
                    message = ticket_id.ToString()
                });
            });
        }

        partial void BeforeDelete(dm.Ticket delete);

    }
}

