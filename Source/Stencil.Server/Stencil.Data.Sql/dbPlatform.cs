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
    
    public partial class dbPlatform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dbPlatform()
        {
            this.ProductVersionPlatforms = new HashSet<dbProductVersionPlatform>();
        }
    
        public System.Guid platform_id { get; set; }
        public string platform_name { get; set; }
        public int bitness { get; set; }
        public System.DateTimeOffset created_utc { get; set; }
        public System.DateTimeOffset updated_utc { get; set; }
        public Nullable<System.DateTimeOffset> deleted_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_hydrate_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_success_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_invalid_utc { get; set; }
        public Nullable<System.DateTimeOffset> sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dbProductVersionPlatform> ProductVersionPlatforms { get; set; }
    }
}
