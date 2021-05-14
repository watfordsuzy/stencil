using Codeable.Foundation.Common;
using Codeable.Foundation.Core.Caching;
using Codeable.Foundation.Core.Unity;
using Newtonsoft.Json;
using Stencil.Common;
using Stencil.Common.Configuration;
using Stencil.Primary;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Plugins.GitHub.Integration
{
    public class GitHubJsonWebHookValidator : IGitHubWebHookValidator
    {
        private const string SignaturePrefix = "sha256=";

        private readonly IFoundation _foundation;

        protected string EnvironmentName
        {
            get
            {
                return this.Cache.PerLifetime(nameof(EnvironmentName), delegate ()
                {
                    return _foundation.Resolve<ISettingsResolver>().GetSetting(CommonAssumptions.APP_KEY_ENVIRONMENT);
                });
            }
        }

        protected string GitHubSecret
        {
            get
            {
                return this.Cache.PerLifetime(nameof(GitHubSecret), delegate ()
                {
                    return _foundation.Resolve<StencilAPI>().Direct.GlobalSettings.GetValueOrDefault(string.Format(CommonAssumptions.CONFIG_KEY_GITHUB_SECRET, this.EnvironmentName), string.Empty);
                });
            }
        }

        protected virtual AspectCache Cache { get; set; }

        public GitHubJsonWebHookValidator(IFoundation foundation)
        {
            _foundation = foundation ?? throw new ArgumentNullException(nameof(foundation));

            this.Cache = new AspectCache(nameof(GitHubJsonWebHookValidator), foundation, new ExpireStaticLifetimeManager($"{nameof(GitHubJsonWebHookValidator)}.Life15", TimeSpan.FromMinutes(15), false));
        }

        public async Task<GitHubWebHookValidationResult> ValidateEventPayloadAsync(HttpRequestMessage request)
        {
            var result = GitHubWebHookValidationResult.Failed();

            string eventName = request.Headers.GetValues("X-GitHub-Event").FirstOrDefault();
            string signatureWithPrefix = request.Headers.GetValues("X-Hub-Signature-256").FirstOrDefault();
            string delivery = request.Headers.GetValues("X-GitHub-Delivery").FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(eventName))
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(signatureWithPrefix))
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(delivery))
            {
                return result;
            }

            using (MemoryStream ms = new MemoryStream())
            {

                // Save the request body to a memory stream so we can calculate
                // the hash of the body before we actually deserialize the payload.
                await request.Content.CopyToAsync(ms);

                ms.Position = 0;

                string signatureWithPrefixString = signatureWithPrefix;
                if (signatureWithPrefixString.StartsWith(SignaturePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    string signature = signatureWithPrefixString.Substring("sha256=".Length);
                    byte[] secret = Encoding.ASCII.GetBytes(GitHubSecret);
                    using (var hmac = new HMACSHA256(secret))
                    {
                        byte[] hash = hmac.ComputeHash(ms);
                        string hashString = ToHexString(hash);
                        if (hashString.Equals(signature))
                        {
                            ms.Position = 0;

                            // Only provide the payload to the caller if we've validated it.
                            result = new GitHubWebHookValidationResult(eventName, ms.ToArray());
                        }
                    }
                }
            }

            return result;

            string ToHexString(byte[] bytes)
            {
                var builder = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                {
                    builder.AppendFormat("{0:x2}", b);
                }

                return builder.ToString();
            }
        }
    }
}