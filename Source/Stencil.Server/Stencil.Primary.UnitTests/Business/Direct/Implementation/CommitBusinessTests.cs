using Stencil.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using dm = Stencil.Domain;

namespace Stencil.Primary.Business.Direct.Implementation
{
    public class CommitBusinessTests : BusinessTestBase
    {
        [Theory]
        [InlineData(5, 1, 1)]
        [InlineData(5, -1, 0)]
        [InlineData(5, 0, 0)]
        [InlineData(5, 10, 5)]
        [InlineData(20, 10, 10)]
        public void GetCommitsPendingIngestion_Gets_The_Requested_Amount_Or_Fewer_If_Less_Available(int count, int take, int expected)
        {
            AddRandomCommits(count);

            var commitBusiness = new CommitBusiness(_foundation.Object);

            IEnumerable<dm.Commit> commits = commitBusiness.GetCommitsPendingIngestion(take);

            Assert.Equal(expected, commits.Count());
        }

        [Theory]
        [InlineData(5, 1, 1)]
        [InlineData(5, 2, 2)]
        [InlineData(5, -1, 0)]
        [InlineData(5, 0, 0)]
        [InlineData(10, 10, 5)]
        [InlineData(20, 10, 10)]
        public void GetCommitsPendingIngestion_Skips_Ingested_Commits(int count, int take, int expected)
        {
            AddRandomCommits(count, (commit, index) =>
            {
                // Mark half of all commits as ingested
                if (index % 2 == 0)
                {
                    commit.commit_message_decoded_utc = DateTimeOffset.UtcNow;
                }
            });

            var commitBusiness = new CommitBusiness(_foundation.Object);

            IEnumerable<dm.Commit> commits = commitBusiness.GetCommitsPendingIngestion(take);

            // half of all of the commits are already decoded
            Assert.Equal(expected, commits.Count());
        }

        [Fact]
        public void MarkCommitAsIngested_Skips_Unknown_Commits()
        {
            var commitBusiness = new CommitBusiness(_foundation.Object);

            commitBusiness.MarkCommitAsIngested(Guid.Empty);

            commitBusiness.MarkCommitAsIngested(Guid.NewGuid());
        }

        [Fact]
        public void MarkCommitAsIngested_Marks_Ingested_Commits()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var commit = new dbCommit
            {
                commit_id = Guid.NewGuid(),
                commit_ref = "0",
                commit_author_name = "author",
                commit_author_email = "author@example.com",
                commit_message = "Commit Message",
                commit_message_decoded_utc = null,
            };

            _context.dbCommits.Add(commit);

            _context.SaveChanges();

            var commitBusiness = new CommitBusiness(_foundation.Object);

            commitBusiness.MarkCommitAsIngested(commit.commit_id);

            Assert.True(_context.dbCommits.Find(commit.commit_id).commit_message_decoded_utc >= now);
        }

        [Fact]
        public void MarkCommitAsIngested_Marks_Ingested_Commits_Again()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var commit = new dbCommit
            {
                commit_id = Guid.NewGuid(),
                commit_ref = "0",
                commit_author_name = "author",
                commit_author_email = "author@example.com",
                commit_message = "Commit Message",
                commit_message_decoded_utc = now.Subtract(TimeSpan.FromHours(3)),
            };

            _context.dbCommits.Add(commit);

            _context.SaveChanges();

            var commitBusiness = new CommitBusiness(_foundation.Object);

            commitBusiness.MarkCommitAsIngested(commit.commit_id);

            Assert.True(_context.dbCommits.Find(commit.commit_id).commit_message_decoded_utc >= now);
        }

        private void AddRandomCommits(int count, Action<dbCommit, int> onCommit = null)
        {
            foreach (var index in Enumerable.Range(0, count))
            {
                var commit = new dbCommit
                {
                    commit_id = Guid.NewGuid(),
                    commit_ref = $"{index:00000000}",
                    commit_author_name = $"author-{index}",
                    commit_author_email = $"author-{index}@example.com",
                    commit_message = index % 3 == 0 ? $"{index} message" : null,
                };

                onCommit?.Invoke(commit, index);

                _context.dbCommits.Add(commit);
            }

            _context.SaveChanges();
        }
    }
}
