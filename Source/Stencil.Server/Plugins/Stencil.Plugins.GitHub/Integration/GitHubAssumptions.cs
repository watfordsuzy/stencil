using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stencil.Plugins.GitHub.Integration
{
    public static class GitHubAssumptions
    {
        public static readonly string HEADER_EVENT_NAME = "X-GitHub-Event";
        public static readonly string HEADER_SIGNATURE = "X-Hub-Signature-256";
        public static readonly string HEADER_DELIVERY = "X-GitHub-Delivery";

        public static readonly string SIGNATURE_PREFIX = "sha256=";
    }
}