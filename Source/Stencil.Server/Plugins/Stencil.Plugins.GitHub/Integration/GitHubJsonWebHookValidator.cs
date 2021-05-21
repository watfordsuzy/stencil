using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using Codeable.Foundation.Core.Caching;
using Codeable.Foundation.Core.Unity;
using Stencil.Common;
using Stencil.Common.Configuration;
using Stencil.Primary;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Plugins.GitHub.Integration
{
    public class GitHubJsonWebHookValidator : ChokeableClass, IGitHubWebHookValidator
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
            : base(foundation)
        {
            _foundation = foundation ?? throw new ArgumentNullException(nameof(foundation));

            this.Cache = new AspectCache(
                nameof(GitHubJsonWebHookValidator),
                foundation,
                new ExpireStaticLifetimeManager($"{nameof(GitHubJsonWebHookValidator)}.Life15", TimeSpan.FromMinutes(15), false));
        }

        public Task<GitHubWebHookValidationResult> ValidateEventPayloadAsync(HttpRequestMessage request)
        {
            return base.ExecuteFunction(nameof(ValidateEventPayloadAsync), async delegate ()
            {
                if (!GitHubEventHeaders.TryParse(request, out GitHubEventHeaders headers))
                {
                    return GitHubWebHookValidationResult.Failed();
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    // Save the request body to a memory stream so we can calculate
                    // the hash of the body before we actually deserialize the payload.
                    await request.Content.CopyToAsync(ms);

                    ms.Position = 0;

                    string signatureWithPrefixString = headers.SignatureWithPrefix;
                    if (signatureWithPrefixString.StartsWith(GitHubAssumptions.SIGNATURE_PREFIX, StringComparison.OrdinalIgnoreCase))
                    {
                        string signature = signatureWithPrefixString.Substring(GitHubAssumptions.SIGNATURE_PREFIX.Length);
                        byte[] secret = Encoding.ASCII.GetBytes(this.GitHubSecret);
                        using (var hmac = new HMACSHA256(secret))
                        {
                            byte[] hash = hmac.ComputeHash(ms);
                            string hashString = ToHexString(hash);
                            if (hashString.Equals(signature, StringComparison.OrdinalIgnoreCase))
                            {
                                ms.Position = 0;

                                // Only provide the payload to the caller if we've validated it.
                                return new GitHubWebHookValidationResult(headers.Delivery, headers.EventName, ms.ToArray());
                            }
                        }
                    }
                }

                return GitHubWebHookValidationResult.Failed();
            });

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