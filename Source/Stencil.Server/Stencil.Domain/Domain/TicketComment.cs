//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Core;
using System;
using System.Collections.Generic;
using System.Text;


namespace Stencil.Domain
{
    public partial class TicketComment : DomainModel
    {	
        public TicketComment()
        {
				
        }
    
        public Guid ticket_comment_id { get; set; }
        public Guid ticket_id { get; set; }
        public Guid commenter_id { get; set; }
        public DateTime commented_on_utc { get; set; }
        public string ticket_comment { get; set; }
        public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }
        public DateTime? deleted_utc { get; set; }
        public DateTime? sync_success_utc { get; set; }
        public DateTime? sync_invalid_utc { get; set; }
        public DateTime? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }
	}
}

