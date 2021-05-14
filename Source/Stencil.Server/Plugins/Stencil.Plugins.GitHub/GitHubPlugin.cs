using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Core;
using Codeable.Foundation.UI.Web.Common.Plugins;
using Codeable.Foundation.UI.Web.Core;
using Stencil.Common;
using Stencil.Common.Configuration;
using Stencil.Common.Integration;
using Stencil.Primary.Integration;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Stencil.Plugins.GitHub.Integration;
using Stencil.Plugins.GitHub.Workers;

namespace Stencil.Plugins.GitHub
{
    public class GitHubPlugin : ChokeableClass, IWebPlugin
    {
        public GitHubPlugin()
            : base(CoreFoundation.Current)
        {
        }

        public string DisplayName => "GitHub";

        public string DisplayVersion => WebCoreUtility.GetInformationalVersion(Assembly.GetExecutingAssembly());

        public bool WebInitialize(IFoundation foundation, IDictionary<string, string> pluginConfig)
        {
            base.IFoundation = foundation;
            base.IFoundation.Container.RegisterType<IGitHubWebHookValidator, GitHubJsonWebHookValidator>(new ContainerControlledLifetimeManager());
            base.IFoundation.Container.RegisterType<ICommitMessageParser, CommitMessageParser>(new ContainerControlledLifetimeManager());

            return true;
        }

        public object InvokeCommand(string name, Dictionary<string, object> caseInsensitiveParameters)
        {
            return null;
        }

        public void RegisterCustomRouting(System.Web.Routing.RouteCollection routes)
        {
            base.ExecuteMethod(nameof(RegisterCustomRouting), delegate ()
            {
                IDaemonManager daemonManager = this.IFoundation.GetDaemonManager();
                ISettingsResolver settingsResolver = this.IFoundation.Resolve<ISettingsResolver>();
                bool isBackPane = settingsResolver.IsBackPane();
                bool isLocalHost = settingsResolver.IsLocalHost();
                bool isHydrate = settingsResolver.IsHydrate();
                if (isBackPane && !isLocalHost && !isHydrate)
                {
                    /*
                    DaemonConfig config = new DaemonConfig()
                    {
                        InstanceName = AmazonEncodingDaemon.DAEMON_NAME,
                        ContinueOnError = true,
                        IntervalMilliSeconds = (int)TimeSpan.FromMinutes(1).TotalMilliseconds,
                        StartDelayMilliSeconds = 15,
                        TaskConfiguration = string.Empty
                    };
                    daemonManager.RegisterDaemon(config, new AmazonEncodingDaemon(this.IFoundation), true);

                    config = new DaemonConfig()
                    {
                        InstanceName = AmazonImageResizeDaemon.DAEMON_NAME,
                        ContinueOnError = true,
                        IntervalMilliSeconds = (int)TimeSpan.FromMinutes(1).TotalMilliseconds,
                        StartDelayMilliSeconds = 15,
                        TaskConfiguration = string.Empty
                    };
                    daemonManager.RegisterDaemon(config, new AmazonImageResizeDaemon(this.IFoundation), true);
                    */
                }

            });
        }

        public void UnRegisterCustomRouting(System.Web.Routing.RouteCollection routes)
        {
        }

        public void RegisterLegacyOverrides(LegacyOverrideCollection overrides)
        {
        }

        public void UnRegisterLegacyOverrides(LegacyOverrideCollection overrides)
        {
        }

        public int DesiredRegistrationPriority => 0;

        public void OnWebPluginRegistered(IWebPlugin plugin)
        {
        }

        public void OnAfterWebPluginsRegistered(IEnumerable<IWebPlugin> allWebPlugins)
        {
        }

        public void OnWebPluginUnRegistered(IWebPlugin iWebPlugin)
        {
        }

        public void OnAfterWebPluginsUnRegistered(IWebPlugin[] iWebPlugin)
        {
        }

        public T RetrieveMetaData<T>(string token) => default;
    }
}