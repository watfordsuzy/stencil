using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stencil.Plugins.GitHub.Integration
{
    public class GitHubWebHookValidationResult
    {
        public bool Success { get; }

        public string EventId { get; }

        public string EventName { get; }

        public byte[] Payload { get; }

        private GitHubWebHookValidationResult()
        {
            Success = false;
        }

        public GitHubWebHookValidationResult(string eventId, string eventName, byte[] payload)
        {
            Success = true;
            EventId = eventId ?? throw new ArgumentNullException(nameof(eventId));
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        public static GitHubWebHookValidationResult Failed()
            => new GitHubWebHookValidationResult();
    }
}