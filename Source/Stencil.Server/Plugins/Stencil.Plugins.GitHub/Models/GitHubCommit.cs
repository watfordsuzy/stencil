using System.Collections.Generic;

namespace Stencil.Plugins.GitHub.Models
{
    public class GitHubCommit
    {
        public string id { get; set; }

        public string timestamp { get; set; }

        public string message { get; set; }

        public GitHubCommitAuthor author { get; set; }

        public string url { get; set; }

        public bool distinct { get; set; }

        public IList<string> added { get; set; }

        public IList<string> modified { get; set; }

        public IList<string> removed { get; set; }
    }
}