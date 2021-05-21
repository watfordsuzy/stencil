using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common.Configuration;
using Stencil.Plugins.GitHub.Integration;
using Stencil.Plugins.GitHub.Models;
using Stencil.Plugins.GitHub.UnitTests;
using Stencil.Primary;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Mapping;
using System;
using System.Threading;
using Xunit;

using dm = Stencil.Domain;
using sdk = Stencil.SDK.Models;

namespace Stencil.Plugins.GitHub.Daemons
{
    public class GitHubTicketInProgressDaemonTests
    {
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<ISettingsResolver> _settingsResolver;
        protected readonly Mock<IDaemonManager> _daemonManager;
        protected readonly Mock<ICommitMessageParser> _commitParser;

        public GitHubTicketInProgressDaemonTests()
        {
            _foundation = new Mock<IFoundation>();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _settingsResolver = new Mock<ISettingsResolver>();
            _daemonManager = new Mock<IDaemonManager>();
            _commitParser = new Mock<ICommitMessageParser>();

            _container = new UnityContainer();
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<ISettingsResolver>(_settingsResolver.Object);
            _container.RegisterInstance<StencilAPI>(new StencilAPI(_foundation.Object));
            _container.RegisterInstance<IDaemonManager>(_daemonManager.Object);
            _container.RegisterInstance<ICommitMessageParser>(_commitParser.Object);

            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());
            _foundation.Setup(ff => ff.GetDaemonManager())
                       .Returns(_daemonManager.Object);

            Mapper.AddProfile<PrimaryMappingProfile>();
        }

        [Fact]
        public void Execute_Exits_If_Cancelled()
        {
            var commitBusiness = new Mock<ICommitBusiness>();
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();

            var daemon = new GitHubTicketInProgressDaemon(_foundation.Object);

            daemon.Execute(_foundation.Object, cts.Token);

            commitBusiness.Verify(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void Execute_Exits_If_No_Commits()
        {
            var commitBusiness = new Mock<ICommitBusiness>();
            commitBusiness.Setup(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()))
                          .Returns(Array.Empty<dm.Commit>());
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            var daemon = new GitHubTicketInProgressDaemon(_foundation.Object);

            daemon.Execute(_foundation.Object, default);

            commitBusiness.Verify(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void Execute_Marks_Commits_As_Ingested_Without_Matches()
        {
            var commits = new[]
            {
                new dm.Commit
                {
                    commit_id = Guid.NewGuid(),
                    commit_message = "0",
                },
                new dm.Commit
                {
                    commit_id = Guid.NewGuid(),
                    commit_message = "1",
                },
            };

            var commitBusiness = new Mock<ICommitBusiness>();
            commitBusiness.SetupSequence(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()))
                          .Returns(commits)
                          .Returns(Array.Empty<dm.Commit>());
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            // No tickets in these commit messages
            _commitParser.Setup(cc => cc.FindTicketIdCandidates(It.IsAny<string>()))
                         .Returns(Array.Empty<Guid>());

            var daemon = new GitHubTicketInProgressDaemon(_foundation.Object);

            daemon.Execute(_foundation.Object, default);

            commitBusiness.Verify(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()), Times.Exactly(2));
            commitBusiness.Verify(cc => cc.MarkCommitAsIngested(commits[0].commit_id), Times.Once());
            commitBusiness.Verify(cc => cc.MarkCommitAsIngested(commits[1].commit_id), Times.Once());
        }

        [Fact]
        public void Execute_Marks_Tickets_As_InProgress_And_Commits_As_Ingested()
        {
            var commits = new[]
            {
                new dm.Commit
                {
                    commit_id = Guid.NewGuid(),
                    commit_message = "0",
                },
                new dm.Commit
                {
                    commit_id = Guid.NewGuid(),
                    commit_message = "1",
                },
            };

            var tickets = new[]
            {
                new[] { Guid.NewGuid(), Guid.NewGuid(), },
                new[] { Guid.NewGuid(), },
            };

            var commitBusiness = new Mock<ICommitBusiness>();
            commitBusiness.SetupSequence(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()))
                          .Returns(commits)
                          .Returns(Array.Empty<dm.Commit>());
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            var ticketBusiness = new Mock<ITicketBusiness>();
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            _commitParser.SetupSequence(cc => cc.FindTicketIdCandidates(It.IsAny<string>()))
                         .Returns(tickets[0])
                         .Returns(tickets[1]);

            var daemon = new GitHubTicketInProgressDaemon(_foundation.Object);

            daemon.Execute(_foundation.Object, default);

            commitBusiness.Verify(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()), Times.Exactly(2));
            for (int ii = 0; ii < commits.Length; ++ii)
            {
                for (int jj = 0; jj < tickets[ii].Length; ++jj)
                {
                    ticketBusiness.Verify(tt => tt.MarkTicketAsInProgress(tickets[ii][jj], commits[ii].commit_id), Times.Once());
                }

                commitBusiness.Verify(cc => cc.MarkCommitAsIngested(commits[ii].commit_id), Times.Once());
                commitBusiness.Verify(cc => cc.MarkCommitAsIngested(commits[ii].commit_id), Times.Once());
            }
        }

        [Fact]
        public void Execute_Stops_When_Cancelled()
        {
            var commits = new[]
                        {
                new dm.Commit
                {
                    commit_id = Guid.NewGuid(),
                    commit_message = "0",
                },
                new dm.Commit
                {
                    commit_id = Guid.NewGuid(),
                    commit_message = "1",
                },
            };

            var tickets = new[]
            {
                new[] { Guid.NewGuid(), Guid.NewGuid(), },
                new[] { Guid.NewGuid(), },
            };

            int cancelledAfter = 0;

            using var cts = new CancellationTokenSource();

            var commitBusiness = new Mock<ICommitBusiness>();
            commitBusiness.SetupSequence(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()))
                          .Returns(commits)
                          .Returns(Array.Empty<dm.Commit>());
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            var ticketBusiness = new Mock<ITicketBusiness>();
            _container.RegisterInstance<ITicketBusiness>(ticketBusiness.Object);

            _commitParser.SetupSequence(cc => cc.FindTicketIdCandidates(It.IsAny<string>()))
                         .Returns(() =>
                         {
                             // Cancel after the first commit is parsed
                             cts.Cancel();
                             return tickets[0];
                         })
                         .Returns(tickets[1]);

            var daemon = new GitHubTicketInProgressDaemon(_foundation.Object);

            daemon.Execute(_foundation.Object, cts.Token);

            // We should be cancelled before we get the next set of commits
            commitBusiness.Verify(cc => cc.GetCommitsPendingIngestion(It.IsAny<int>()), Times.Exactly(1));

            for (int ii = 0; ii < commits.Length; ++ii)
            {
                // Determine how many times a method will be called based on whether or not
                // the commit was before or after the cancellation point
                Times times = ii <= cancelledAfter ? Times.Once() : Times.Never();

                for (int jj = 0; jj < tickets[ii].Length; ++jj)
                {
                    ticketBusiness.Verify(tt => tt.MarkTicketAsInProgress(tickets[ii][jj], commits[ii].commit_id), times);
                }

                commitBusiness.Verify(cc => cc.MarkCommitAsIngested(commits[ii].commit_id), times);
            }
        }
    }
}
