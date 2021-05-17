using AutoMapper;
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Daemons;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Microsoft.Practices.Unity;
using Moq;
using Stencil.Common.Configuration;
using Stencil.Plugins.RestAPI.UnitTests;
using Stencil.Primary;
using Stencil.Primary.Mapping;
using Stencil.Web.Controllers;
using Stencil.Web.Security;
using System;
using dm = Stencil.Domain;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public abstract class ControllerTestBase<TController> : IDisposable
        where TController : RestApiBaseController
    {
        protected readonly Mock<IHandleExceptionProvider> _exceptionHandler;
        protected readonly UnityContainer _container;
        protected readonly Mock<IFoundation> _foundation;
        protected readonly Mock<ISettingsResolver> _settingsResolver;
        protected readonly Mock<IDaemonManager> _daemonManager;

        protected readonly string _baseUri;
        protected readonly dm.Account _adminAccount;
        protected readonly dm.Account _userAccount;

        protected ControllerTestBase()
        {
            _foundation = new Mock<IFoundation>();

            _exceptionHandler = new Mock<IHandleExceptionProvider>();
            _settingsResolver = new Mock<ISettingsResolver>();
            _daemonManager = new Mock<IDaemonManager>();

            _container = new UnityContainer();
            _container.RegisterInstance<IFoundation>(_foundation.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(_exceptionHandler.Object);
            _container.RegisterInstance<IHandleExceptionProvider>(Assumptions.SWALLOWED_EXCEPTION_HANDLER, _exceptionHandler.Object);
            _container.RegisterInstance<ISettingsResolver>(_settingsResolver.Object);
            _container.RegisterInstance<StencilAPI>(new StencilAPI(_foundation.Object));
            _container.RegisterInstance<IDaemonManager>(_daemonManager.Object);

            _foundation.Setup(ff => ff.Container)
                       .Returns(_container);
            _foundation.Setup(ff => ff.GetAspectCoordinator())
                       .Returns(new TestAspectCoordinator());
            _foundation.Setup(ff => ff.GetDaemonManager())
                       .Returns(_daemonManager.Object);

            _baseUri = "https://test.example.com/api";
            _adminAccount = new dm.Account
            {
                account_id = Guid.NewGuid(),
                email = "admin@example.com",
                entitlements = dm.WellKnownEntitlements.super_admin.ToString(),
            };
            _userAccount = new dm.Account
            {
                account_id = Guid.NewGuid(),
                email = "user@example.com",
                entitlements = String.Empty,
            };

            Mapper.AddProfile<PrimaryMappingProfile>();
        }

        protected TController CreateGet(string route, dm.Account account = null)
            => CreateController(route, account, System.Net.Http.HttpMethod.Get);

        protected TController CreatePost(string route, dm.Account account = null)
            => CreateController(route, account, System.Net.Http.HttpMethod.Post);

        protected TController CreatePut(string route, dm.Account account = null)
            => CreateController(route, account, System.Net.Http.HttpMethod.Put);

        protected TController CreateDelete(string route, dm.Account account = null)
            => CreateController(route, account, System.Net.Http.HttpMethod.Delete);

        protected abstract TController CreateController();

        protected TController CreateController(string route, dm.Account account = null, System.Net.Http.HttpMethod method = null)
        {
            var controller = CreateController();
            SetCurrentRequest(controller, route, method);
            SetCurrentAccount(controller, account);
            return controller;
        }

        protected void SetCurrentRequest(TController controller, string route, System.Net.Http.HttpMethod method = null)
        {
            controller.Configuration = new System.Web.Http.HttpConfiguration
            {
            };
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                Method = method ?? System.Net.Http.HttpMethod.Get,
                RequestUri = new Uri($"{_baseUri.TrimEnd('/')}/{route.TrimStart('/')}".TrimEnd('/')),
            };
        }

        protected void SetCurrentAccount(TController controller, dm.Account account)
        {
            controller.Request.Properties[ApiKeyHttpAuthorize.CURRENT_ACCOUNT_HTTP_CONTEXT_KEY] = account;
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
