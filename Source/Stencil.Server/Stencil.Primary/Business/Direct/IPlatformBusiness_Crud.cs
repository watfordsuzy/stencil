//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Stencil.Domain;

namespace Stencil.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface IPlatformBusiness
    {
        Platform GetById(Guid platform_id);
        List<Platform> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        Platform Insert(Platform insertPlatform);
        Platform Update(Platform updatePlatform);
        
        void Delete(Guid platform_id);
        void SynchronizationUpdate(Guid platform_id, bool success, DateTime sync_date_utc, string sync_log);
        List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid platform_id, bool success, DateTime sync_date_utc, string sync_log);
        List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid platform_id, string reason);
        
    }
}

