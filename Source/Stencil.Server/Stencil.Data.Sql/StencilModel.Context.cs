﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Stencil.Data.Sql
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class StencilContext : DbContext
    {
        public StencilContext()
            : base("name=StencilContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<dbAccount> dbAccounts { get; set; }
        public virtual DbSet<dbGlobalSetting> dbGlobalSettings { get; set; }
        public virtual DbSet<dbAsset> dbAssets { get; set; }
        public virtual DbSet<dbPlatform> dbPlatforms { get; set; }
        public virtual DbSet<dbProduct> dbProducts { get; set; }
        public virtual DbSet<dbProductVersion> dbProductVersions { get; set; }
        public virtual DbSet<dbTicket> dbTickets { get; set; }
        public virtual DbSet<dbProductVersionPlatform> dbProductVersionPlatforms { get; set; }
        public virtual DbSet<dbAffectedProduct> dbAffectedProducts { get; set; }
    
        public virtual ObjectResult<Nullable<System.Guid>> spAccount_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spAccount_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spAccount_HydrateSyncUpdate(Nullable<System.Guid> account_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var account_idParameter = account_id.HasValue ?
                new ObjectParameter("account_id", account_id) :
                new ObjectParameter("account_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAccount_HydrateSyncUpdate", account_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spAccount_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spAccount_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spAccount_SyncUpdate(Nullable<System.Guid> account_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var account_idParameter = account_id.HasValue ?
                new ObjectParameter("account_id", account_id) :
                new ObjectParameter("account_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAccount_SyncUpdate", account_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    
        public virtual int spIndex_InvalidateAggregates()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spIndex_InvalidateAggregates");
        }
    
        public virtual int spIndex_InvalidateAll()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spIndex_InvalidateAll");
        }
    
        public virtual ObjectResult<spIndex_Status_Result> spIndex_Status()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spIndex_Status_Result>("spIndex_Status");
        }
    
        public virtual int spIndexHydrate_InvalidateAggregates()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spIndexHydrate_InvalidateAggregates");
        }
    
        public virtual int spIndexHydrate_InvalidateAll()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spIndexHydrate_InvalidateAll");
        }
    
        public virtual ObjectResult<spIndexHydrate_Status_Result> spIndexHydrate_Status()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spIndexHydrate_Status_Result>("spIndexHydrate_Status");
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spPlatform_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spPlatform_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spPlatform_HydrateSyncUpdate(Nullable<System.Guid> platform_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var platform_idParameter = platform_id.HasValue ?
                new ObjectParameter("platform_id", platform_id) :
                new ObjectParameter("platform_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spPlatform_HydrateSyncUpdate", platform_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spPlatform_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spPlatform_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spPlatform_SyncUpdate(Nullable<System.Guid> platform_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var platform_idParameter = platform_id.HasValue ?
                new ObjectParameter("platform_id", platform_id) :
                new ObjectParameter("platform_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spPlatform_SyncUpdate", platform_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spProduct_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spProduct_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spProduct_HydrateSyncUpdate(Nullable<System.Guid> product_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var product_idParameter = product_id.HasValue ?
                new ObjectParameter("product_id", product_id) :
                new ObjectParameter("product_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spProduct_HydrateSyncUpdate", product_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spProduct_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spProduct_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spProduct_SyncUpdate(Nullable<System.Guid> product_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var product_idParameter = product_id.HasValue ?
                new ObjectParameter("product_id", product_id) :
                new ObjectParameter("product_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spProduct_SyncUpdate", product_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spProductVersion_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spProductVersion_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spProductVersion_HydrateSyncUpdate(Nullable<System.Guid> product_version_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var product_version_idParameter = product_version_id.HasValue ?
                new ObjectParameter("product_version_id", product_version_id) :
                new ObjectParameter("product_version_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spProductVersion_HydrateSyncUpdate", product_version_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spProductVersion_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spProductVersion_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spProductVersion_SyncUpdate(Nullable<System.Guid> product_version_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var product_version_idParameter = product_version_id.HasValue ?
                new ObjectParameter("product_version_id", product_version_id) :
                new ObjectParameter("product_version_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spProductVersion_SyncUpdate", product_version_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spTicket_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spTicket_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spTicket_HydrateSyncUpdate(Nullable<System.Guid> ticket_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var ticket_idParameter = ticket_id.HasValue ?
                new ObjectParameter("ticket_id", ticket_id) :
                new ObjectParameter("ticket_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spTicket_HydrateSyncUpdate", ticket_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spTicket_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spTicket_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spTicket_SyncUpdate(Nullable<System.Guid> ticket_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var ticket_idParameter = ticket_id.HasValue ?
                new ObjectParameter("ticket_id", ticket_id) :
                new ObjectParameter("ticket_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spTicket_SyncUpdate", ticket_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spProductVersionPlatform_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spProductVersionPlatform_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spProductVersionPlatform_HydrateSyncUpdate(Nullable<System.Guid> product_version_platform_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var product_version_platform_idParameter = product_version_platform_id.HasValue ?
                new ObjectParameter("product_version_platform_id", product_version_platform_id) :
                new ObjectParameter("product_version_platform_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spProductVersionPlatform_HydrateSyncUpdate", product_version_platform_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spProductVersionPlatform_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spProductVersionPlatform_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spProductVersionPlatform_SyncUpdate(Nullable<System.Guid> product_version_platform_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var product_version_platform_idParameter = product_version_platform_id.HasValue ?
                new ObjectParameter("product_version_platform_id", product_version_platform_id) :
                new ObjectParameter("product_version_platform_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spProductVersionPlatform_SyncUpdate", product_version_platform_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spAffectedProduct_HydrateSyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spAffectedProduct_HydrateSyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spAffectedProduct_HydrateSyncUpdate(Nullable<System.Guid> affected_product_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_hydrate_utc, string sync_log)
        {
            var affected_product_idParameter = affected_product_id.HasValue ?
                new ObjectParameter("affected_product_id", affected_product_id) :
                new ObjectParameter("affected_product_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_hydrate_utcParameter = sync_hydrate_utc.HasValue ?
                new ObjectParameter("sync_hydrate_utc", sync_hydrate_utc) :
                new ObjectParameter("sync_hydrate_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAffectedProduct_HydrateSyncUpdate", affected_product_idParameter, sync_successParameter, sync_hydrate_utcParameter, sync_logParameter);
        }
    
        public virtual ObjectResult<Nullable<System.Guid>> spAffectedProduct_SyncGetInvalid(Nullable<int> allowableSecondsToProcessIndex, string sync_agent)
        {
            var allowableSecondsToProcessIndexParameter = allowableSecondsToProcessIndex.HasValue ?
                new ObjectParameter("allowableSecondsToProcessIndex", allowableSecondsToProcessIndex) :
                new ObjectParameter("allowableSecondsToProcessIndex", typeof(int));
    
            var sync_agentParameter = sync_agent != null ?
                new ObjectParameter("sync_agent", sync_agent) :
                new ObjectParameter("sync_agent", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("spAffectedProduct_SyncGetInvalid", allowableSecondsToProcessIndexParameter, sync_agentParameter);
        }
    
        public virtual int spAffectedProduct_SyncUpdate(Nullable<System.Guid> affected_product_id, Nullable<bool> sync_success, Nullable<System.DateTimeOffset> sync_success_utc, string sync_log)
        {
            var affected_product_idParameter = affected_product_id.HasValue ?
                new ObjectParameter("affected_product_id", affected_product_id) :
                new ObjectParameter("affected_product_id", typeof(System.Guid));
    
            var sync_successParameter = sync_success.HasValue ?
                new ObjectParameter("sync_success", sync_success) :
                new ObjectParameter("sync_success", typeof(bool));
    
            var sync_success_utcParameter = sync_success_utc.HasValue ?
                new ObjectParameter("sync_success_utc", sync_success_utc) :
                new ObjectParameter("sync_success_utc", typeof(System.DateTimeOffset));
    
            var sync_logParameter = sync_log != null ?
                new ObjectParameter("sync_log", sync_log) :
                new ObjectParameter("sync_log", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAffectedProduct_SyncUpdate", affected_product_idParameter, sync_successParameter, sync_success_utcParameter, sync_logParameter);
        }
    }
}
