using Codeable.Foundation.Common;
using Stencil.Plugins.GitHub.Models;
using Stencil.Primary;
using Stencil.Primary.Daemons;
using System;

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
            this.CommitMessageParser = iFoundation.Resolve<ICommitMessageParser>();
        }

        public StencilAPI API { get; set; }

        public ICommitMessageParser CommitMessageParser { get; set; }

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
                    foreach (Guid ticket_id in this.CommitMessageParser.FindTicketIdCandidates(commit.message))
                    {
                        // TODO: transition a ticket to InProgress once the first commit hits
                    }
                }
            });
        }
    }
}