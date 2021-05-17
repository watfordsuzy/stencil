using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Stencil.Plugins.GitHub.Integration
{
    public class GitHubEventHeaders
    {
        public string EventName { get; }

        public string SignatureWithPrefix { get; }

        public string Delivery { get; }

        private GitHubEventHeaders(string eventName, string signature, string delivery)
        {
            EventName = eventName;
            SignatureWithPrefix = signature;
            Delivery = delivery;
        }

        public static bool TryParse(HttpRequestMessage request, out GitHubEventHeaders eventHeaders)
        {
            eventHeaders = default;

            if (!request.Headers.TryGetValues(GitHubAssumptions.HEADER_EVENT_NAME, out IEnumerable<string> eventNameHeaderValues))
            {
                return false;
            }

            if (!request.Headers.TryGetValues(GitHubAssumptions.HEADER_SIGNATURE, out IEnumerable<string> signatureHeaderValues))
            {
                return false;
            }

            if (!request.Headers.TryGetValues(GitHubAssumptions.HEADER_DELIVERY, out IEnumerable<string> deliveryHeaderValues))
            {
                return false;
            }

            string eventName = eventNameHeaderValues.FirstOrDefault();
            string signatureWithPrefix = signatureHeaderValues.FirstOrDefault();
            string delivery = deliveryHeaderValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(eventName)
             || string.IsNullOrWhiteSpace(signatureWithPrefix)
             || string.IsNullOrWhiteSpace(delivery))
            {
                return false;
            }

            eventHeaders = new GitHubEventHeaders(eventName, signatureWithPrefix, delivery);
            return true;
        }
    }
}