using Stencil.Data.Sql;
using Stencil.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public partial class CommitBusiness
    {
        public IEnumerable<Commit> GetCommitsPendingIngestion(int take = 10)
        {
            if (take <= 0)
            {
                return Enumerable.Empty<Commit>();
            }

            using (StencilContext db = this.CreateSQLContext())
            {
                return db.dbCommits.Where(cc => cc.commit_message_decoded_utc == null)
                                   .OrderBy(cc => cc.created_utc)
                                   .Take(take)
                                   .ToList()
                                   .Select(cc => cc.ToDomainModel())
                                   .ToList();
            }
        }

        public void MarkCommitAsIngested(Guid commit_id)
        {
            using (StencilContext db = this.CreateSQLContext())
            {
                dbCommit commit = db.dbCommits.FirstOrDefault(cc => cc.commit_id == commit_id);
                if (commit != null)
                {
                    commit.commit_message_decoded_utc = DateTime.UtcNow;

                    db.SaveChanges();
                }
            }
        }
    }
}
