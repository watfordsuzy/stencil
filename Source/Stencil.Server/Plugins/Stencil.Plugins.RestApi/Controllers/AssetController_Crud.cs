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
    [RoutePrefix("api/assets")]
    public partial class AssetController : HealthRestApiController
    {
        public AssetController(IFoundation foundation)
            : base(foundation, "Asset")
        {
        }

        [HttpGet]
        [Route("{asset_id}")]
        public object GetById(Guid asset_id)
        {
            return base.ExecuteFunction<object>("GetById", delegate()
            {
                this.BeforeGet();

                dm.Asset result = this.API.Direct.Assets.GetById(asset_id);
                if (result == null)
                {
                    return Http404("Asset");
                }

                

                this.AfterGet(result);

                return base.Http200(new ItemResult<sdk.Asset>()
                {
                    success = true,
                    item = result.ToSDKModel()
                });
            });
        }

        partial void BeforeGet();
        partial void AfterGet(dm.Asset result);
        partial void AfterGet(List<dm.Asset> result);
        
        
        
        
       

        [HttpPost]
        [Route("")]
        public object Create(sdk.Asset asset)
        {
            return base.ExecuteFunction<object>("Create", delegate()
            {
                this.ValidateNotNull(asset, "Asset");

                this.BeforeInsert(asset);

                dm.Asset insert = asset.ToDomainModel();
              
                insert = this.API.Direct.Assets.Insert(insert);
                
                this.AfterInsert(asset, insert);

                
                sdk.Asset result = insert.ToSDKModel();

                return base.Http201(new ItemResult<sdk.Asset>()
                {
                    success = true,
                    item = result
                }
                , string.Format("api/asset/{0}", asset.asset_id));

            });

        }

        partial void BeforeInsert(sdk.Asset asset);
        partial void AfterInsert(sdk.Asset asset, dm.Asset inserted);

        [HttpPut]
        [Route("{asset_id}")]
        public object Update(Guid asset_id, sdk.Asset asset)
        {
            return base.ExecuteFunction<object>("Update", delegate()
            {
                this.ValidateNotNull(asset, "Asset");
                this.ValidateRouteMatch(asset_id, asset.asset_id, "Asset");

                asset.asset_id = asset_id;

                this.BeforeUpdate(asset);

                dm.Asset update = asset.ToDomainModel();

                update = this.API.Direct.Assets.Update(update);

                this.AfterUpdate(asset, update);
                
                
                sdk.Asset existing = this.API.Direct.Assets.GetById(update.asset_id).ToSDKModel();
                
                return base.Http200(new ItemResult<sdk.Asset>()
                {
                    success = true,
                    item = existing
                });

            });

        }

        partial void BeforeUpdate(sdk.Asset asset);
        partial void AfterUpdate(sdk.Asset asset, dm.Asset updated);

        

        [HttpDelete]
        [Route("{asset_id}")]
        public object Delete(Guid asset_id)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.Asset delete = this.API.Direct.Assets.GetById(asset_id);
                if (delete == null)
                {
                    return base.Http404(@"Asset");
                }
                
                this.BeforeDelete(delete);
                
                this.API.Direct.Assets.Delete(asset_id);

                return base.Http200(new ActionResult()
                {
                    success = true,
                    message = asset_id.ToString()
                });
            });
        }

        partial void BeforeDelete(dm.Asset delete);

    }
}

