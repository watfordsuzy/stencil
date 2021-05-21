using Stencil.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Direct
{
    public partial interface ICommitBusiness
    {
        IEnumerable<Commit> GetCommitsPendingIngestion(int take = 10);

        void MarkCommitAsIngested(Guid commit_id);
    }
}
