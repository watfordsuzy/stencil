using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Core.Caching;
using Codeable.Foundation.Core.Unity;
using Stencil.Plugins.GitHub.Workers;
using Stencil.Primary;
using Stencil.Primary.Daemons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

using dm = Stencil.Domain;

namespace Stencil.Plugins.GitHub.Daemons
{
    public class GitHubTicketInProgressDaemon : NonReentrantDaemon
    {
        public StencilAPI API { get; set; }

        public AspectCache Cache { get; set; }

        public ICommitMessageParser CommitMessageParser { get; set; }

        public override DaemonSynchronizationPolicy SynchronizationPolicy => DaemonSynchronizationPolicy.SingleAppDomain;

        public override string DaemonName => nameof(GitHubTicketInProgressDaemon);

        public GitHubTicketInProgressDaemon(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.API = iFoundation.Resolve<StencilAPI>();
            this.Cache = new AspectCache(
                nameof(GitHubTicketInProgressDaemon),
                iFoundation,
                new ExpireStaticLifetimeManager($"{nameof(GitHubTicketInProgressDaemon)}.Life15", TimeSpan.FromMinutes(15), false));
            this.CommitMessageParser = iFoundation.Resolve<ICommitMessageParser>();
        }

        protected override void ExecuteNonReentrant(IFoundation foundation, CancellationToken token)
        {
            base.ExecuteMethod(nameof(ExecuteNonReentrant), delegate ()
            {
                while (!token.IsCancellationRequested)
                {
                    bool ingestedAtLeastOneCommit = false;
                    foreach (dm.Commit commit in this.API.Direct.Commits.GetCommitsPendingIngestion())
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        foreach (Guid ticketId in this.CommitMessageParser.FindTicketIdCandidates(commit.commit_message))
                        {
                            this.API.Direct.Tickets.MarkTicketAsInProgress(ticketId, commit.commit_id);
                        }

                        this.API.Direct.Commits.MarkCommitAsIngested(commit.commit_id);

                        ingestedAtLeastOneCommit = true;
                    }

                    if (!ingestedAtLeastOneCommit)
                    {
                        break;
                    }
                }
            });

        }
    }
}