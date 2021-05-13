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
    
    public partial class dbProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dbProduct()
        {
            this.ProductVersions = new HashSet<dbProductVersion>();
            this.AffectedProducts = new HashSet<dbAffectedProduct>();
        }
    
        public System.Guid product_id { get; set; }
        public string product_name { get; set; }
        public System.Guid product_owner_id { get; set; }
        public string product_description { get; set; }
        public System.DateTimeOffset created_utc { get; set; }
        public System.DateTimeOffset updated_utc { get; set; }
        public Nullable<System.DateTimeOffset> deleted_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_hydrate_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_success_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_invalid_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }
    
        public virtual dbAccount Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dbProductVersion> ProductVersions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dbAffectedProduct> AffectedProducts { get; set; }
    }
}
