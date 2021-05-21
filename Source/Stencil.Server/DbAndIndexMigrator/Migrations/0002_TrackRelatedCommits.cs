using Codeable.Foundation.Common;
using Nest;
using SimpleMigrations;

namespace DbAndIndexMigrator.DbMigrations
{
    [Migration(Version, "Add a table to track incoming commits")]
    public class _0002_TrackRelatedCommits : StencilDbMigrationBase
    {
        public const int Version = 2;

        public _0002_TrackRelatedCommits(IFoundation foundation)
            : base(foundation)
        {
        }

        protected override void UpDatabase()
        {
            this.Execute(@"
CREATE TABLE [dbo].[Commit] (
	 [commit_id] uniqueidentifier NOT NULL
    ,[commit_ref] nvarchar(64) NOT NULL
    ,[commit_author_name] nvarchar(256) NOT NULL
    ,[commit_author_email] nvarchar(256) NOT NULL
    ,[commit_message_decoded_utc] datetimeoffset(0) NULL
    ,[commit_message] nvarchar(max) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
  ,CONSTRAINT [PK_Commit] PRIMARY KEY CLUSTERED 
  (
	  [commit_id] ASC
  )
)
");
        }

        protected override void UpIndex(ElasticClient client)
        {
            // Do Nothing
        }
    }
}
