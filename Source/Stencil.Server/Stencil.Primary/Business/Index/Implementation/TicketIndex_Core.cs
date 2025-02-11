//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using Stencil.SDK;
using sdk = Stencil.SDK.Models;
using Stencil.SDK.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Index.Implementation
{
    public partial class TicketIndex : IndexerBase<sdk.Ticket>, ITicketIndex
    {
        public TicketIndex(IFoundation foundation)
            : base(foundation, "TicketIndex", DocumentNames.Ticket)
        {

        }
        protected override string GetModelId(sdk.Ticket model)
        {
            return model.ticket_id.ToString();
        }
        public ListResult<sdk.Ticket> GetByReportedByID(Guid account_id, int skip, int take, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("GetByReportedByID", delegate ()
            {
                QueryContainer query = Query<sdk.Ticket>.Term(w => w.reported_by_id, account_id);

                

                int takePlus = take;
                if(take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }
                
                List<SortFieldDescriptor<sdk.Ticket>> sortFields = new List<SortFieldDescriptor<sdk.Ticket>>();
                if(!string.IsNullOrEmpty(order_by))
                {
                    SortFieldDescriptor<sdk.Ticket> item = new SortFieldDescriptor<sdk.Ticket>()
                        .Field(order_by)
                        .Order(descending ? SortOrder.Descending : SortOrder.Ascending);
                        
                    sortFields.Add(item);
                }
                SortFieldDescriptor<sdk.Ticket> defaultSort = new SortFieldDescriptor<sdk.Ticket>()
                    .Field(r => r.ticket_id)
                    .Ascending();
                
                sortFields.Add(defaultSort);
                
                ElasticClient client = this.ClientFactory.CreateClient();
                ISearchResponse<sdk.Ticket> searchResponse = client.Search<sdk.Ticket>(s => s
                    .Query(q => query)
                    .Skip(skip)
                    .Take(takePlus)
                    .Sort(sr => sr.Multi(sortFields))
                    .Type(this.DocumentType));

                ListResult<sdk.Ticket> result = searchResponse.Documents.ToSteppedListResult(skip, take, searchResponse.GetTotalHit());
                
                return result;
            });
        }
        public ListResult<sdk.Ticket> GetByAssignedToID(Guid account_id, int skip, int take, string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("GetByAssignedToID", delegate ()
            {
                QueryContainer query = Query<sdk.Ticket>.Term(w => w.assigned_to_id, account_id);

                

                int takePlus = take;
                if(take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }
                
                List<SortFieldDescriptor<sdk.Ticket>> sortFields = new List<SortFieldDescriptor<sdk.Ticket>>();
                if(!string.IsNullOrEmpty(order_by))
                {
                    SortFieldDescriptor<sdk.Ticket> item = new SortFieldDescriptor<sdk.Ticket>()
                        .Field(order_by)
                        .Order(descending ? SortOrder.Descending : SortOrder.Ascending);
                        
                    sortFields.Add(item);
                }
                SortFieldDescriptor<sdk.Ticket> defaultSort = new SortFieldDescriptor<sdk.Ticket>()
                    .Field(r => r.ticket_id)
                    .Ascending();
                
                sortFields.Add(defaultSort);
                
                ElasticClient client = this.ClientFactory.CreateClient();
                ISearchResponse<sdk.Ticket> searchResponse = client.Search<sdk.Ticket>(s => s
                    .Query(q => query)
                    .Skip(skip)
                    .Take(takePlus)
                    .Sort(sr => sr.Multi(sortFields))
                    .Type(this.DocumentType));

                ListResult<sdk.Ticket> result = searchResponse.Documents.ToSteppedListResult(skip, take, searchResponse.GetTotalHit());
                
                return result;
            });
        }
        
        public ListResult<sdk.Ticket> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false, Guid? reported_by_id = null, Guid? assigned_to_id = null)
        {
            return base.ExecuteFunction("Find", delegate ()
            {
                int takePlus = take;
                if(take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }
                
                QueryContainer query = Query<sdk.Ticket>
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields(mf => mf
                                .Field(f => f.ticket_title)
                                .Field(f => f.ticket_description)
                ));
                                
                if(reported_by_id.HasValue)
                {
                    query &= Query<sdk.Ticket>.Term(f => f.reported_by_id, reported_by_id.Value);
                }
                if(assigned_to_id.HasValue)
                {
                    query &= Query<sdk.Ticket>.Term(f => f.assigned_to_id, assigned_to_id.Value);
                }
                
                
                SortOrder sortOrder = SortOrder.Ascending;
                if (descending)
                {
                    sortOrder = SortOrder.Descending;
                }
                if (string.IsNullOrEmpty(order_by))
                {
                    order_by = "";
                }

                ElasticClient client = this.ClientFactory.CreateClient();
                ISearchResponse<sdk.Ticket> searchResponse = client.Search<sdk.Ticket>(s => s
                    .Query(q => query)
                    .Skip(skip)
                    .Take(takePlus)
                    .Sort(r => r.Field(order_by, sortOrder))
                    .Type(this.DocumentType));
                
                ListResult<sdk.Ticket> result = searchResponse.Documents.ToSteppedListResult(skip, take, searchResponse.GetTotalHit());
                
                return result;
            });
        }
        

    }
}
