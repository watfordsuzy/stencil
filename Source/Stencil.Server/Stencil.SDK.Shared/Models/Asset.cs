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

namespace Stencil.SDK.Models
{
    public partial class Asset : SDKModel
    {	
        public Asset()
        {
				
        }
    
        public virtual Guid asset_id { get; set; }
        public virtual AssetType type { get; set; }
        public virtual bool available { get; set; }
        public virtual bool resize_required { get; set; }
        public virtual bool encode_required { get; set; }
        public virtual bool resize_processing { get; set; }
        public virtual bool encode_processing { get; set; }
        public virtual string thumb_small_dimensions { get; set; }
        public virtual string thumb_medium_dimensions { get; set; }
        public virtual string thumb_large_dimensions { get; set; }
        public virtual string resize_status { get; set; }
        public virtual int resize_attempts { get; set; }
        public virtual DateTime? resize_attempt_utc { get; set; }
        public virtual string encode_identifier { get; set; }
        public virtual string encode_status { get; set; }
        public virtual string raw_url { get; set; }
        public virtual string public_url { get; set; }
        public virtual string thumb_small_url { get; set; }
        public virtual string thumb_medium_url { get; set; }
        public virtual string thumb_large_url { get; set; }
        public virtual string encode_log { get; set; }
        public virtual string resize_log { get; set; }
        public virtual Dependency dependencies { get; set; }
        public virtual int encode_attempts { get; set; }
        public virtual DateTime? encode_attempt_utc { get; set; }
        public virtual string resize_mode { get; set; }
        
	}
}

