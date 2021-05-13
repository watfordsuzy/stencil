using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Workers.Models
{
    public class AssignProductOwnerRequest
    {
        public Guid ticket_id { get; set; }

        public Guid product_id { get; set; }
    }
}
