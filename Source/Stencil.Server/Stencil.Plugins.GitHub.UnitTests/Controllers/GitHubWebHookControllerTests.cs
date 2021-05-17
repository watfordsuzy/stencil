using Codeable.Foundation.Common;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Plugins.GitHub.Daemons;
using Stencil.Plugins.GitHub.Integration;
using Stencil.Plugins.GitHub.Models;
using Stencil.Plugins.GitHub.Workers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stencil.Plugins.GitHub.Controllers
{
    public class GitHubWebHookControllerTests : ControllerTestBase<GitHubWebHookController>
    {
        protected override GitHubWebHookController CreateController() => new(_foundation.Object);

        [Fact]
        public async Task WebHook_Returns_BadRequest_If_Invalid_Request_Signature()
        {
            string eventId = Guid.NewGuid().ToString();
            string eventName = GitHubPushEvent.EventName;
            byte[] contentBytes = await base.GetFixtureBytesAsync($@"{nameof(WebHook_Returns_BadRequest_If_Invalid_Request_Signature)}.json");

            var validator = new Mock<IGitHubWebHookValidator>();
            validator.Setup(vv => vv.ValidateEventPayloadAsync(It.IsAny<HttpRequestMessage>()))
                     .Returns(Task.FromResult(GitHubWebHookValidationResult.Failed()));
            _container.RegisterInstance<IGitHubWebHookValidator>(validator.Object);

            var controller = CreatePost("github/webhook");

            controller.Request.Headers.Add(GitHubAssumptions.HEADER_EVENT_NAME, eventName);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_DELIVERY, eventId);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}0123456789abcdef");
            controller.Request.Content = new ByteArrayContent(contentBytes)
            {
                Headers =
                {
                    { "Content-Type", "application/json" },
                },
            };

            var response = await controller.GitHubWebhookAsync();

            _ = HttpAssert.IsBadRequest(response);
        }

        [Fact]
        public async Task WebHook_Returns_BadRequest_If_Invalid_Json()
        {
            string eventId = Guid.NewGuid().ToString();
            string eventName = GitHubPushEvent.EventName;
            byte[] contentBytes = await base.GetFixtureBytesAsync($@"{nameof(WebHook_Returns_BadRequest_If_Invalid_Json)}.json");

            var validator = new Mock<IGitHubWebHookValidator>();
            validator.Setup(vv => vv.ValidateEventPayloadAsync(It.IsAny<HttpRequestMessage>()))
                     .Returns(Task.FromResult(new GitHubWebHookValidationResult(eventId, eventName, contentBytes)));
            _container.RegisterInstance<IGitHubWebHookValidator>(validator.Object);

            var controller = CreatePost("github/webhook");

            controller.Request.Headers.Add(GitHubAssumptions.HEADER_EVENT_NAME, eventName);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_DELIVERY, eventId);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}0123456789abcdef");
            controller.Request.Content = new ByteArrayContent(contentBytes)
            {
                Headers =
                {
                    { "Content-Type", "application/json" },
                },
            };

            var response = await controller.GitHubWebhookAsync();

            _ = HttpAssert.IsBadRequest(response);
        }

        [Fact]
        public async Task WebHook_Skips_Other_Event_Types()
        {
            string eventId = Guid.NewGuid().ToString();
            string eventName = "random event type";
            byte[] contentBytes = await base.GetFixtureBytesAsync($@"{nameof(WebHook_Skips_Other_Event_Types)}.json");

            var validator = new Mock<IGitHubWebHookValidator>();
            validator.Setup(vv => vv.ValidateEventPayloadAsync(It.IsAny<HttpRequestMessage>()))
                     .Returns(Task.FromResult(new GitHubWebHookValidationResult(eventId, eventName, contentBytes)));
            _container.RegisterInstance<IGitHubWebHookValidator>(validator.Object);

            var controller = CreatePost("github/webhook");

            controller.Request.Headers.Add(GitHubAssumptions.HEADER_EVENT_NAME, eventName);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_DELIVERY, eventId);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}0123456789abcdef");
            controller.Request.Content = new ByteArrayContent(contentBytes)
            {
                Headers =
                {
                    { "Content-Type", "application/json" },
                },
            };

            var response = await controller.GitHubWebhookAsync();

            _ = HttpAssert.IsSuccess(response);

            _daemonManager.Verify(dd => dd.GetRegisteredDaemonTask(nameof(GitHubPushEventWorker)), Times.Never());
        }

        [Fact]
        public async Task WebHook_Enqueues_Work_For_Valid_Events()
        {
            string eventId = Guid.NewGuid().ToString();
            string eventName = GitHubPushEvent.EventName;
            byte[] contentBytes = await base.GetFixtureBytesAsync($@"{nameof(WebHook_Enqueues_Work_For_Valid_Events)}.json");

            var validator = new Mock<IGitHubWebHookValidator>();
            validator.Setup(vv => vv.ValidateEventPayloadAsync(It.IsAny<HttpRequestMessage>()))
                     .Returns(Task.FromResult(new GitHubWebHookValidationResult(eventId, eventName, contentBytes)));
            _container.RegisterInstance<IGitHubWebHookValidator>(validator.Object);

            var worker = new TestGitHubPushEventWorker(_foundation.Object);
            _daemonManager.Setup(dd => dd.GetRegisteredDaemonTask(nameof(GitHubPushEventWorker)))
                          .Returns(worker);

            var controller = CreatePost("github/webhook");

            controller.Request.Headers.Add(GitHubAssumptions.HEADER_EVENT_NAME, eventName);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_DELIVERY, eventId);
            controller.Request.Headers.Add(GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}0123456789abcdef");
            controller.Request.Content = new ByteArrayContent(contentBytes)
            {
                Headers =
                {
                    { "Content-Type", "application/json" },
                },
            };

            var response = await controller.GitHubWebhookAsync();

            _ = HttpAssert.IsSuccess(response);

            // Get the enqueued events and make sure we've got all the right commit data
            Assert.Collection(
                worker.GetEnqueuedEvents(),
                evt =>
                {
                    Assert.Collection(
                        evt.commits,
                        c0 =>
                        {
                            Assert.Equal(@"c441029cf673f84c8b7db52d0a5944ee5c52ff89", c0.id);
                            Assert.Equal(@"Test", c0.message);
                            Assert.Equal(@"Test 0 Person", c0.author.name);
                            Assert.Equal(@"author0@example.com", c0.author.email);
                        },
                        c1 =>
                        {
                            Assert.Equal(@"36c5f2243ed24de58284a96f2a643bed8c028658", c1.id);
                            Assert.Equal(@"This is me testing the windows client.", c1.message);
                            Assert.Equal(@"Test 1 Person", c1.author.name);
                            Assert.Equal(@"author1@example.com", c1.author.email);
                        },
                        c2 =>
                        {
                            Assert.Equal(@"1481a2de7b2a7d02428ad93446ab166be7793fbb", c2.id);
                            Assert.Equal(@"Rename madame-bovary.txt to words/madame-bovary.txt", c2.message);
                            Assert.Equal(@"Test 2 Person", c2.author.name);
                            Assert.Equal(@"author2@example.com", c2.author.email);
                        });
                });
        }

        /// <summary>
        /// Provides a way to capture the enqueued events without executing the worker
        /// </summary>
        private class TestGitHubPushEventWorker : GitHubPushEventWorker
        {
            public TestGitHubPushEventWorker(IFoundation foundation)
                : base(foundation)
            {
            }

            public IEnumerable<GitHubPushEvent> GetEnqueuedEvents()
                => base.RequestQueue.ToArray();
        }

        [Fact]
        public void Agitate_Spins_Up_The_Daemon()
        {
            var controller = CreatePost("github/agitate");

            var response = controller.AgitateDaemons("codeable", "commits");

            _ = HttpAssert.IsSuccess(response);

            _daemonManager.Verify(dd => dd.StartDaemon(GitHubTicketInProgressDaemon.DAEMON_NAME), Times.Once());
        }
    }
}
