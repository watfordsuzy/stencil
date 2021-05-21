using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common.Configuration;
using Stencil.Plugins.GitHub.Models;
using Stencil.Plugins.GitHub.UnitTests;
using Stencil.Primary;
using Stencil.Primary.Business.Direct;
using Stencil.Primary.Mapping;
using System;
using Xunit;

using dm = Stencil.Domain;

namespace Stencil.Plugins.GitHub.Workers
{
    public class GitHubPushEventWorkerTests
    {
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<ISettingsResolver> _settingsResolver;
        protected readonly Mock<IDaemonManager> _daemonManager;

        public GitHubPushEventWorkerTests()
        {
            _foundation = new Mock<IFoundation>();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _settingsResolver = new Mock<ISettingsResolver>();
            _daemonManager = new Mock<IDaemonManager>();

            _container = new UnityContainer();
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<ISettingsResolver>(_settingsResolver.Object);
            _container.RegisterInstance<StencilAPI>(new StencilAPI(_foundation.Object));
            _container.RegisterInstance<IDaemonManager>(_daemonManager.Object);

            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());
            _foundation.Setup(ff => ff.GetDaemonManager())
                       .Returns(_daemonManager.Object);

            Mapper.AddProfile<PrimaryMappingProfile>();
        }

        [Fact]
        public void ProcessRequest_Ignores_Null_Or_Empty_Requests()
        {
            var commitBusiness = new Mock<ICommitBusiness>();
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            var worker = new GitHubPushEventWorker(_foundation.Object);

            worker.EnqueueRequest(null);
            worker.EnqueueRequest(new GitHubPushEvent());
            worker.EnqueueRequest(new GitHubPushEvent
                {
                    commits = Array.Empty<GitHubCommit>(),
                });

            worker.Execute(_foundation.Object, default);

            commitBusiness.Verify(tt => tt.Insert(It.IsAny<dm.Commit>()), Times.Never());
        }

        [Fact]
        public void ProcessRequest_Marks_Commits_As_Ingested()
        {
            var commitBusiness = new Mock<ICommitBusiness>();
            _container.RegisterInstance<ICommitBusiness>(commitBusiness.Object);

            var worker = new GitHubPushEventWorker(_foundation.Object);

            GitHubPushEvent request = new GitHubPushEvent
            {
                @ref = "abc",
                base_ref = "def",
                before = "abc",
                after = "000",
                created = true,
                commits = new[]
                {
                    new GitHubCommit
                    {
                        id = "0",
                        author = new GitHubCommitAuthor
                        {
                            name = "author0",
                            email = "author0@example.com",
                        },
                        message = "TEST Message",
                    },
                },
            };
            worker.EnqueueRequest(request);

            worker.Execute(_foundation.Object, default);

            commitBusiness.Verify(
                tt => tt.Insert(
                    It.Is<dm.Commit>(
                        cc => cc.commit_ref == request.commits[0].id
                           && cc.commit_author_name == request.commits[0].author.name
                           && cc.commit_author_email == request.commits[0].author.email
                           && cc.commit_message == request.commits[0].message)),
                Times.Once());
        }
    }
}
