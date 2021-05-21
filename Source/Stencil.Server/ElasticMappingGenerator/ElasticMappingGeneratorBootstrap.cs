using Codeable.Foundation.Common;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Codeable.Foundation.Core.System;
using Microsoft.Practices.Unity;
using Stencil.Common.Configuration;
using Stencil.Primary.Business.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticMappingGenerator
{
    public class ElasticMappingGeneratorBootstrap : CoreBootStrap
    {
        public override void OnAfterSelfRegisters(IFoundation foundation)
        {
            base.OnAfterSelfRegisters(foundation);

            foundation.Container.RegisterType<ISettingsResolver, AppConfigSettingsResolver>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IStencilElasticClientFactory, MappingGeneratorElasticClientFactory>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<MappingGeneratorElasticClientFactory>(new ContainerControlledLifetimeManager());

            // Replace Exception Handlers
            foundation.Container.RegisterType<IHandleException, StandardThrowExceptionHandler>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IHandleExceptionProvider, StandardThrowExceptionHandlerProvider>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterInstance<StandardThrowExceptionHandlerProvider>(new StandardThrowExceptionHandlerProvider(foundation.GetLogger()), new ContainerControlledLifetimeManager());
        }
    }
}
