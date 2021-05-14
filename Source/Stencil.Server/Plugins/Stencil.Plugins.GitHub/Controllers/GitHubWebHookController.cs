using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Core.Caching;
using Codeable.Foundation.Core.Unity;
using Stencil.Common;
using Stencil.Common.Configuration;
using Stencil.Common.Integration;
using Stencil.Domain;
using Stencil.Primary;
using Stencil.Primary.Health;
using Stencil.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stencil.Plugins.GitHub.Integration;
using Stencil.Plugins.GitHub.Models;
using System.Threading.Tasks;
using Stencil.Plugins.GitHub.Workers;
using System.IO;

namespace Stencil.Plugins.GitHub.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/github")]
    public class GitHubWebHookController : RestApiBaseController
    {
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public GitHubWebHookController(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.Cache = new AspectCache(nameof(GitHubWebHookController), iFoundation, new ExpireStaticLifetimeManager($"{nameof(GitHubWebHookController)}.Life15", System.TimeSpan.FromMinutes(15), false));
        }

        public AspectCache Cache { get; set; }

        protected string EnvironmentName
        {
            get
            {
                return this.Cache.PerLifetime("EnvironmentName", delegate ()
                {
                    return this.IFoundation.Resolve<ISettingsResolver>().GetSetting(CommonAssumptions.APP_KEY_ENVIRONMENT);
                });
            }
        }

        protected int MaximumRetries
        {
            get
            {
                return this.Cache.PerLifetime("MaximumRetries", delegate ()
                {
                    int maxRetries;
                    int.TryParse(this.API.Direct.GlobalSettings.GetValueOrDefault(CommonAssumptions.CONFIG_KEY_AMAZON_ENCODE_RETRY_MAX, "5"), out maxRetries);
                    return maxRetries;
                });
            }
        }

        [HttpPost]
        [Route("webhook")]
        public Task<HttpResponseMessage> GitHubWebhookAsync()
        {
            return base.ExecuteFunction(nameof(GitHubWebhookAsync), async delegate ()
            {
                IGitHubWebHookValidator validator = this.Foundation.Resolve<IGitHubWebHookValidator>();

                GitHubWebHookValidationResult result = await validator.ValidateEventPayloadAsync(this.Request);
                if (!result.Success)
                {
                    return this.Http400("Invalid Payload");
                }

                // Handle push events from GitHub
                if (String.Equals(result.EventName, GitHubPushEvent.EventName, StringComparison.OrdinalIgnoreCase))
                {
                    GitHubPushEvent pushEvent;
                    try
                    {
                        using (var ms = new MemoryStream(result.Payload))
                        using (var reader = new JsonTextReader(new StreamReader(ms)))
                        {
                            pushEvent = _serializer.Deserialize<GitHubPushEvent>(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Foundation.LogError(ex, "Could not parse GitHub push event payload");
                        return this.Http400("Invalid Payload");
                    }

                    GitHubPushEventWorker.EnqueueRequest(this.Foundation, pushEvent);
                }

                return this.Http200("OK", "");
            });
        }

        [HttpPost]
        [Route("agitate")]
        public object AgitateDaemons(string key, string type)
        {
            return base.ExecuteFunction("AgitateDaemons", delegate ()
            {
                if (key == "codeable")
                {
                    IDaemonManager daemonManager = this.IFoundation.GetDaemonManager();
                    /*if (type == "photo")
                    {
                        daemonManager.StartDaemon(AmazonImageResizeDaemon.DAEMON_NAME);
                    }
                    if (type == "video")
                    {
                        daemonManager.StartDaemon(AmazonEncodingDaemon.DAEMON_NAME);
                    }*/
                }
                return this.Http200("OK", "");
            });
        }

    }
}