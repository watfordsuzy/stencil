//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class dbAffectedProduct
    {
        public System.Guid affected_product_id { get; set; }
        public System.Guid ticket_id { get; set; }
        public System.Guid product_id { get; set; }
        public System.DateTimeOffset created_utc { get; set; }
        public System.DateTimeOffset updated_utc { get; set; }
        public Nullable<System.DateTimeOffset> deleted_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_hydrate_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_success_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_invalid_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }
    
        public virtual dbProduct Product { get; set; }
        public virtual dbTicket Ticket { get; set; }
    }
}
