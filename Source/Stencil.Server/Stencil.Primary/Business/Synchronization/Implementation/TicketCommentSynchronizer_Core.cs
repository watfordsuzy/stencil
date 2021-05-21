//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using Stencil.Common;
using Stencil.Domain;
using Stencil.Primary.Business.Index;
using Stencil.Primary.Health;
using sdk = Stencil.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Codeable.Foundation.Core;

namespace Stencil.Primary.Synchronization.Implementation
{
    public partial class TicketCommentSynchronizer : SynchronizerBase<Guid>, ITicketCommentSynchronizer
    {
        public TicketCommentSynchronizer(IFoundation foundation)
            : base(foundation, "TicketCommentSynchronizer")
        {

        }

        public override int Priority
        {
            get
            {
                return 70;
            }
        }

        public override void PerformSynchronizationForItem(Guid primaryKey)
        {
            base.ExecuteMethod("PerformSynchronizationForItem", delegate ()
            {
                TicketComment domainModel = this.API.Direct.TicketComments.GetById(primaryKey);
                if (domainModel != null)
                {
                    Action<Guid, bool, DateTime, string> synchronizationUpdateMethod = this.API.Direct.TicketComments.SynchronizationUpdate;
                    if(this.API.Integration.SettingsResolver.IsHydrate())
                    {
                        synchronizationUpdateMethod = this.API.Direct.TicketComments.SynchronizationHydrateUpdate;
                    }
                    DateTime syncDate = DateTime.UtcNow;
                    if (domainModel.sync_invalid_utc.HasValue)
                    {
                        syncDate = domainModel.sync_invalid_utc.Value;
                    }
                    try
                    {
                        sdk.TicketComment sdkModel = domainModel.ToSDKModel();
                        
                        this.HydrateSDKModelComputed(domainModel, sdkModel);
                        this.HydrateSDKModel(domainModel, sdkModel);

                        if (domainModel.deleted_utc.HasValue)
                        {
                            this.API.Index.TicketComments.DeleteDocument(sdkModel);
                            synchronizationUpdateMethod(domainModel.ticket_comment_id, true, syncDate, null);
                        }
                        else
                        {
                            IndexResult result = this.API.Index.TicketComments.UpdateDocument(sdkModel);
                            if (result.success)
                            {
                                synchronizationUpdateMethod(domainModel.ticket_comment_id, true, syncDate, result.ToString());
                            }
                            else
                            {
                                throw new Exception(result.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.IFoundation.LogError(ex, "PerformSynchronizationForItem");
                        HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_ERROR_SYNC, this.EntityName), 0, 1);
                        synchronizationUpdateMethod(primaryKey, false, syncDate, CoreUtility.FormatException(ex));
                    }
                }
            });
        }

        public override int PerformSynchronization(string requestedAgentName)
        {
            return base.ExecuteFunction("PerformSynchronization", delegate ()
            {
                string agentName = requestedAgentName;
                if(string.IsNullOrEmpty(agentName))
                {
                    agentName = this.AgentName;
                }
                List<Guid?> invalidItems = new List<Guid?>();
                if(this.API.Integration.SettingsResolver.IsHydrate())
                {
                    invalidItems = this.API.Direct.TicketComments.SynchronizationHydrateGetInvalid(CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                else
                {
                    invalidItems = this.API.Direct.TicketComments.SynchronizationGetInvalid(CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                foreach (Guid? item in invalidItems)
                {
                    this.PerformSynchronizationForItem(item.GetValueOrDefault());
                }
                return invalidItems.Count;
            });
        }
        
        /// <summary>
        /// Computed and Calculated Aggs, Typically Generated
        /// </summary>
        protected void HydrateSDKModelComputed(TicketComment domainModel, sdk.TicketComment sdkModel)
        {
            
        }
        partial void HydrateSDKModel(TicketComment domainModel, sdk.TicketComment sdkModel);
    }
}

