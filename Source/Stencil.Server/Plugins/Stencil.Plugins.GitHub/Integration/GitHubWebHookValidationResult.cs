using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stencil.Plugins.GitHub.Integration
{
    public class GitHubWebHookValidationResult
    {
        public bool Success { get; }

        public string EventName { get; }

        public byte[] Payload { get; }

        private GitHubWebHookValidationResult()
        {
            Success = false;
        }

        public GitHubWebHookValidationResult(string eventName, byte[] payload)
        {
            Success = true;
            EventName = eventName;
            Payload = payload;
        }

        public static GitHubWebHookValidationResult Failed()
            => new GitHubWebHookValidationResult();
    }
}