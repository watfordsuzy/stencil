using Codeable.Foundation.Common;
using Stencil.Plugins.GitHub.Models;
using Stencil.Primary;
using Stencil.Primary.Daemons;
using System;

using dm = Stencil.Domain;

namespace Stencil.Plugins.GitHub.Workers
{
    public class GitHubPushEventWorker : WorkerBase<GitHubPushEvent>
    {
        public static void EnqueueRequest(IFoundation foundation, GitHubPushEvent request)
        {
            EnqueueRequest<GitHubPushEventWorker>(foundation, WORKER_NAME, request, (int)TimeSpan.FromMinutes(2).TotalMilliseconds); // updates every 2 mins
        }

        public const string WORKER_NAME = nameof(GitHubPushEventWorker);

        public GitHubPushEventWorker(IFoundation iFoundation)
            : base(iFoundation, WORKER_NAME)
        {
            this.API = iFoundation.Resolve<StencilAPI>();
        }

        public StencilAPI API { get; set; }

        protected override void ProcessRequest(GitHubPushEvent request)
        {
            base.ExecuteMethod(nameof(ProcessRequest), delegate ()
            {
                // Do not process a request without any commits
                if (request?.commits == null)
                {
                    return;
                }

                foreach (GitHubCommit commit in request.commits)
                {
                    this.API.Direct.Commits.Insert(new dm.Commit
                    {
                        commit_ref = commit.id,
                        commit_author_name = commit.author?.name ?? "<unknown>",
                        commit_author_email = commit.author?.email ?? "<unknown>",
                        commit_message = commit.message,
                    });
                }
            });
        }
    }
}
