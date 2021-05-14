using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Stencil.Plugins.GitHub.Workers
{
    public class CommitMessageParser : ChokeableClass, ICommitMessageParser
    {
        private static readonly Regex s_messageRegex = new Regex(@"\[([a-fA-F0-9-]+)\]");

        public CommitMessageParser(IFoundation foundation)
            : base(foundation)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Guid> FindTicketIdCandidates(string commitMessage)
        {
            return base.ExecuteFunction(nameof(FindTicketIdCandidates), delegate ()
            {
                // Check if there is no hope of finding a Ticket ID in a commit
                if (String.IsNullOrWhiteSpace(commitMessage)
                    || commitMessage.IndexOf('[') < 0)
                {
                    return Enumerable.Empty<Guid>();
                }

                // Otherwise, return all of the candidate Ticket IDs we found
                return s_messageRegex.Matches(commitMessage)
                                     .Cast<Match>()
                                     .Select(mm =>
                                     {
                                         bool success = Guid.TryParse(mm.Groups[1].Value, out Guid guid);
                                         return new { success, guid };
                                     })
                                     .Where(gg => gg.success)
                                     .Select(gg => gg.guid)
                                     .ToArray();
            });
        }
    }
}