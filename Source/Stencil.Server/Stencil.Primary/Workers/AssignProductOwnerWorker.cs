using Codeable.Foundation.Common;
using Stencil.Primary.Daemons;
using Stencil.Primary.Workers.Models;
using System;

namespace Stencil.Primary.Workers
{
    public class AssignProductOwnerWorker : WorkerBase<AssignProductOwnerRequest>
    {
        public static void EnqueueRequest(IFoundation foundation, AssignProductOwnerRequest request)
        {
            EnqueueRequest<AssignProductOwnerWorker>(foundation, WORKER_NAME, request, (int)TimeSpan.FromMinutes(2).TotalMilliseconds); // updates every 2 mins
        }

        public const string WORKER_NAME = nameof(AssignProductOwnerWorker);

        public AssignProductOwnerWorker(IFoundation iFoundation)
            : base(iFoundation, WORKER_NAME)
        {
            this.API = iFoundation.Resolve<StencilAPI>();
        }

        public StencilAPI API { get; set; }

        protected override void ProcessRequest(AssignProductOwnerRequest request)
        {
            base.ExecuteMethod(nameof(ProcessRequest), delegate ()
            {
                if (request != null)
                {
                    this.API.Direct.Tickets.AssignToProductOwner(request.ticket_id, request.product_id);
                }
            });
        }
    }
}
