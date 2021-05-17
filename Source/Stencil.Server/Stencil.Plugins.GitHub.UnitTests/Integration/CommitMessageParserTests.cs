using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Stencil.Plugins.GitHub.Integration
{
    public class CommitMessageParserTests : IntegrationTestBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                     ")]
        [InlineData("  \r\n        \t           ")]
        public void CommitMessageParser_Returns_Empty_For_Empty_Messages(string commitMessage)
        {
            var parser = new CommitMessageParser(_foundation.Object);

            Assert.Empty(parser.FindTicketIdCandidates(commitMessage));
        }

        [Theory]
        [InlineData("Commit message")]
        [InlineData("One\r\nreally\r\nlong\r\ncommit\r\nmessage")]
        [InlineData("Lots of almost ] tickets but no actual tickets [")]
        [InlineData("Lots of things [BB-123]\r\n\t\t- that look like tickets [TH-123]")]
        [InlineData("Degenerate commit message [[[]]][[][][[[[[]")]
        [InlineData("Degenerate commit message [123456[789abcdef[]aaaaa]11234488][dead-b33f[][][a[1[2[3[]4567")]
        public void CommitMessageParser_Returns_Empty_For_Messages_Without_Tickets(string commitMessage)
        {
            var parser = new CommitMessageParser(_foundation.Object);

            Assert.Empty(parser.FindTicketIdCandidates(commitMessage));
        }

        public static IEnumerable<object[]> CommitMessageParser_Finds_Every_Ticket_TestData()
        {
            string[] messages = new []
            {
                "[$]",
                "[$][$]",
                "[$] [$]",
                "[$]\t[$]",
                "Simple [$]\r\n- Other\r\n- Message\r\n- Details",
                "Complex\r\n- Other [$]\r\n- Message [$]\r\n- Details [$]\r\n- [$]",
                // Lots of not GUIDs
                "Complex [$] [$]\r\n- Other object[]\r\n- Message [[]\r\n- Details ][]]\r\n- [[][]]",
            };

            foreach (string message in messages)
            {
                yield return Build(message, guidFormat: "N");
                yield return Build(message, guidFormat: "D");
                yield return Build(message, guidFormat: "N", toUpper: true);
                yield return Build(message, guidFormat: "D", toUpper: true);
            }

            object[] Build(string message, string guidFormat, bool toUpper = false)
            {
                var regex = new Regex(@"\[(\$)\]");
                var guids = new List<Guid>();
                message = regex.Replace(
                    message,
                    mm =>
                    {
                        Guid guid = Guid.NewGuid();
                        guids.Add(guid);
                        string formattedGuid = "[" + guid.ToString(guidFormat) + "]";
                        return toUpper ? formattedGuid.ToUpper() : formattedGuid;
                    });
                return new object[] { message, guids };
            }
        }

        [Theory]
        [MemberData(nameof(CommitMessageParser_Finds_Every_Ticket_TestData))]
        public void CommitMessageParser_Finds_Every_Ticket(string commitMessage, List<Guid> expectedGuids)
        {
            var parser = new CommitMessageParser(_foundation.Object);

            var ticketIds = parser.FindTicketIdCandidates(commitMessage);
            Assert.Equal(expectedGuids, ticketIds.ToList());
        }
    }
}
