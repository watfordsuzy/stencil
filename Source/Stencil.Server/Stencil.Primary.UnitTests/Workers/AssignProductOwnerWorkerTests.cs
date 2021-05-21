using Microsoft.Practices.Unity;
using Moq;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Workers.Models;
using System;
using Xunit;

namespace Stencil.Primary.Workers
{
    public class AssignProductOwnerWorkerTests : WorkerTestBase
    {
        [Fact]
        public void ProcessRequest_Ignores_Null_Requests()
        {
            var ticketBusiness = new Mock<ITicketBusiness>();
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var worker = new AssignProductOwnerWorker(_foundation.Object);

            worker.EnqueueRequest(null);

            worker.Execute(_foundation.Object, default);

            ticketBusiness.Verify(tt => tt.AssignToProductOwner(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public void ProcessRequest_Passes_Along_Assignment()
        {
            var ticketBusiness = new Mock<ITicketBusiness>();
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            var worker = new AssignProductOwnerWorker(_foundation.Object);

            AssignProductOwnerRequest request = new AssignProductOwnerRequest
            {
                ticket_id = Guid.NewGuid(),
                product_id = Guid.NewGuid(),
            };
            worker.EnqueueRequest(request);

            worker.Execute(_foundation.Object, default);

            ticketBusiness.Verify(tt => tt.AssignToProductOwner(request.ticket_id, request.product_id), Times.Once());
        }
    }
}
