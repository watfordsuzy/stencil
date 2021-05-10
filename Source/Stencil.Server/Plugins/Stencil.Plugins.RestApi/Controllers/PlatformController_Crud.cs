//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using Codeable.Foundation.Core;
using System;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using sdk = Stencil.SDK.Models;
using dm = Stencil.Domain;
using Stencil.Primary;
using Stencil.SDK;
using Stencil.Web.Controllers;
using Stencil.Web.Security;

namespace Stencil.Plugins.RestAPI.Controllers
{
    [ApiKeyHttpAuthorize]
    [RoutePrefix("api/platforms")]
    public partial class PlatformController : HealthRestApiController
    {
        public PlatformController(IFoundation foundation)
            : base(foundation, "Platform")
        {
        }

        [HttpGet]
        [Route("{platform_id}")]
        public object GetById(Guid platform_id)
        {
            return base.ExecuteFunction<object>("GetById", delegate()
            {
                this.BeforeGet();

                sdk.Platform result = this.API.Index.Platforms.GetById(platform_id);
                if (result == null)
                {
                    return Http404("Platform");
                }

                this.AfterGet(result);

                

                return base.Http200(new ItemResult<sdk.Platform>()
                {
                    success = true, 
                    item = result
                });
            });
        }

        partial void BeforeGet();
        partial void AfterGet(sdk.Platform result);
        partial void AfterGet(ListResult<sdk.Platform> result);
        
        
        [HttpGet]
        [Route("")]
        public object Find(int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = "")
        {
            return base.ExecuteFunction<object>("Find", delegate()
            {
                this.BeforeFind();

                
                ListResult<sdk.Platform> result = this.API.Index.Platforms.Find(skip, take, keyword, order_by, descending);
                result.success = true;

                this.AfterFind(result);

                return base.Http200(result);
            });
        }
        
        partial void BeforeFind();
        partial void AfterFind(ListResult<sdk.Platform> result);
        
        
        
        
       

        [HttpPost]
        [Route("")]
        public object Create(sdk.Platform platform)
        {
            return base.ExecuteFunction<object>("Create", delegate()
            {
                this.ValidateNotNull(platform, "Platform");

                this.BeforeInsert(platform);

                dm.Platform insert = platform.ToDomainModel();
              
                insert = this.API.Direct.Platforms.Insert(insert);
                
                this.AfterInsert(platform, insert);

                
                sdk.Platform result = this.API.Index.Platforms.GetById(insert.platform_id);

                return base.Http201(new ItemResult<sdk.Platform>()
                {
                    success = true,
                    item = result
                }
                , string.Format("api/platform/{0}", platform.platform_id));

            });

        }

        partial void BeforeInsert(sdk.Platform platform);
        partial void AfterInsert(sdk.Platform platform, dm.Platform inserted);

        [HttpPut]
        [Route("{platform_id}")]
        public object Update(Guid platform_id, sdk.Platform platform)
        {
            return base.ExecuteFunction<object>("Update", delegate()
            {
                this.ValidateNotNull(platform, "Platform");
                this.ValidateRouteMatch(platform_id, platform.platform_id, "Platform");

                platform.platform_id = platform_id;

                this.BeforeUpdate(platform);

                dm.Platform update = platform.ToDomainModel();

                update = this.API.Direct.Platforms.Update(update);

                this.AfterUpdate(platform, update);
                
                
                sdk.Platform existing = this.API.Index.Platforms.GetById(update.platform_id);
                
                
                return base.Http200(new ItemResult<sdk.Platform>()
                {
                    success = true,
                    item = existing
                });

            });

        }

        partial void BeforeUpdate(sdk.Platform platform);
        partial void AfterUpdate(sdk.Platform platform, dm.Platform updated);

        

        [HttpDelete]
        [Route("{platform_id}")]
        public object Delete(Guid platform_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Platform delete = this.API.Direct.Platforms.GetById(platform_id);
                
                this.BeforeDelete(delete);
                
                this.API.Direct.Platforms.Delete(platform_id);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = platform_id.ToString()
                });
            });
        }

        partial void BeforeDelete(dm.Platform delete);

    }
}

