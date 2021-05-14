using Newtonsoft.Json;
using System.Collections.Generic;

namespace Stencil.Plugins.GitHub.Models
{
    public class GitHubPushEvent
    {
        public static readonly string EventName = @"push";

        [JsonProperty("ref")]
        public string @ref { get; set; }
        public string before { get; set; }
        public string after { get; set; }
        public bool created { get; set; }
        public bool deleted { get; set; }
        public bool forced { get; set; }
        public string base_ref { get; set; }
        public string compare { get; set; }
        public IList<GitHubCommit> commits { get; set; }
        public string head_commit { get; set; }
        public GitHubCommitAuthor pusher { get; set; }
        public GitHubRepository repository { get; set; }
        public GitHubOrganization organization { get; set; }
        public GitHubInstallation installation { get; set; }
        public GitHubUser sender { get; set; }
    }
}