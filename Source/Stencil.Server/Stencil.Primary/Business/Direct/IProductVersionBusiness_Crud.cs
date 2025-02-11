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
    public partial interface IProductVersionBusiness
    {
        ProductVersion GetById(Guid product_version_id);
        List<ProductVersion> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false);
        
        List<ProductVersion> GetByProductID(Guid product_id);
        ProductVersion Insert(ProductVersion insertProductVersion);
        ProductVersion Update(ProductVersion updateProductVersion);
        
        void Delete(Guid product_version_id);
        void SynchronizationUpdate(Guid product_version_id, bool success, DateTime sync_date_utc, string sync_log);
        List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(Guid product_version_id, bool success, DateTime sync_date_utc, string sync_log);
        List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent);
        void Invalidate(Guid product_version_id, string reason);
        
    }
}

