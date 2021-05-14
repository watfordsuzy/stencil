using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stencil.Plugins.GitHub.Workers
{
    public interface ICommitMessageParser
    {
        /// <summary>
        /// Searches through git commit messages for candidate Ticket IDs.
        /// </summary>
        /// <remarks>
        /// A candidate Ticket ID is zero or more hex digits within square brackets,
        /// optionally separated by dashes.
        /// </remarks>
        /// <param name="commitMessage">A commit message.</param>
        /// <returns>An enumeration of possible Ticket IDs mentioned in the commit message.</returns>
        IEnumerable<Guid> FindTicketIdCandidates(string commitMessage);
    }
}