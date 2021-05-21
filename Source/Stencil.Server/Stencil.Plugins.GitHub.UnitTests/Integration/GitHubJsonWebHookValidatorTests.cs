using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common;
using Stencil.Plugins.GitHub.Models;
using Stencil.Primary.Business.Direct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using dm = Stencil.Domain;

namespace Stencil.Plugins.GitHub.Integration
{
    public class GitHubJsonWebHookValidatorTests : IntegrationTestBase
    {
        private const string _environmentName = nameof(GitHubJsonWebHookValidatorTests);
        private readonly string _gitHubSecretName = string.Format(CommonAssumptions.CONFIG_KEY_GITHUB_SECRET, _environmentName);
        private readonly string _gitHubSecret = "very-secret";

        private readonly Mock<IGlobalSettingBusiness> _globalSettings;

        public GitHubJsonWebHookValidatorTests()
            : base()
        {
            _settingsResolver.Setup(ss => ss.GetSetting(CommonAssumptions.APP_KEY_ENVIRONMENT))
                             .Returns(_environmentName);
            _globalSettings = new Mock<IGlobalSettingBusiness>();
            _globalSettings.Setup(gs => gs.GetByName(_gitHubSecretName))
                           .Returns(new dm.GlobalSetting
                           {
                               global_setting_id = Guid.NewGuid(),
                               name = _gitHubSecretName,
                               value = _gitHubSecret,
                           });
            _container.RegisterInstance<IGlobalSettingBusiness>(_globalSettings.Object);
        }

        [Fact]
        public async Task ValidateEventPayloadAsync_Requires_Event_Header()
        {
            var request = BuildRequest();

            var validator = new GitHubJsonWebHookValidator(_foundation.Object);

            GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task ValidateEventPayloadAsync_Requires_Signature_Header()
        {
            var request = BuildRequest(
                new Dictionary<string, string>
                {
                    { GitHubAssumptions.HEADER_EVENT_NAME, GitHubPushEvent.EventName },
                });

            var validator = new GitHubJsonWebHookValidator(_foundation.Object);

            GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task ValidateEventPayloadAsync_Requires_Delivery_Header()
        {
            var request = BuildRequest(
                new Dictionary<string, string>
                {
                    { GitHubAssumptions.HEADER_EVENT_NAME, GitHubPushEvent.EventName },
                    { GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}0a1b2c3d4e5f6" },
                });

            var validator = new GitHubJsonWebHookValidator(_foundation.Object);

            GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task ValidateEventPayloadAsync_Requires_Valid_Signature_Header()
        {
            var request = BuildRequest(
                new Dictionary<string, string>
                {
                    { GitHubAssumptions.HEADER_EVENT_NAME, GitHubPushEvent.EventName },
                    { GitHubAssumptions.HEADER_SIGNATURE, "sha=0a1b2c3d4e5f6" },
                    { GitHubAssumptions.HEADER_DELIVERY, Guid.NewGuid().ToString("N") },
                },
                new StringContent(@"{""message"":""content""}"));

            var validator = new GitHubJsonWebHookValidator(_foundation.Object);

            GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task ValidateEventPayloadAsync_Only_Accepts_Valid_Signatures()
        {
            var request = BuildRequest(
                new Dictionary<string, string>
                {
                    { GitHubAssumptions.HEADER_EVENT_NAME, GitHubPushEvent.EventName },
                    { GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}0a1b2c3d4e5f6" },
                    { GitHubAssumptions.HEADER_DELIVERY, Guid.NewGuid().ToString("N") },
                },
                new StringContent(@"{""message"":""content""}"));

            var validator = new GitHubJsonWebHookValidator(_foundation.Object);

            GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task ValidateEventPayloadAsync_Accepts_Valid_Signatures()
        {
            string eventId = Guid.NewGuid().ToString();
            string content = @"{""message"": ""abracadabra""}";
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            string hexSignature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_gitHubSecret)))
            {
                byte[] hash = hmac.ComputeHash(contentBytes);
                hexSignature = String.Join("", hash.Select(bb => bb.ToString("X2")));
            }

            var request = BuildRequest(
                new Dictionary<string, string>
                {
                    { GitHubAssumptions.HEADER_EVENT_NAME, GitHubPushEvent.EventName },
                    { GitHubAssumptions.HEADER_SIGNATURE, $"{GitHubAssumptions.SIGNATURE_PREFIX}{hexSignature}" },
                    { GitHubAssumptions.HEADER_DELIVERY, eventId },
                },
                new ByteArrayContent(contentBytes) { Headers = { { "Content-Type", "application/json" } } });

            var validator = new GitHubJsonWebHookValidator(_foundation.Object);

            GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(request);

            Assert.True(result.Success);
            Assert.Equal(eventId, result.EventId);
            Assert.Equal(GitHubPushEvent.EventName, result.EventName);
            Assert.Equal(contentBytes, result.Payload);
        }

        private static HttpRequestMessage BuildRequest(IDictionary<string, string> headers = null, HttpContent content = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(@"https://stencil.example.com/api/github/webhook"),
                Content = content,
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            return request;
        }
    }
}
