//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#if WINDOWS_PHONE_APP
using RestSharp.Portable;
#else
using RestSharp;
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stencil.SDK.Models;

namespace Stencil.SDK.Endpoints
{
    public partial class ProductVersionPlatformEndpoint : EndpointBase
    {
        public ProductVersionPlatformEndpoint(StencilSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<ProductVersionPlatform>> GetProductVersionPlatformAsync(Guid product_version_platform_id)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "productversionplatforms/{product_version_platform_id}";
            request.AddUrlSegment("product_version_platform_id", product_version_platform_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<ProductVersionPlatform>>(request);
        }
        
        
        public Task<ListResult<ProductVersionPlatform>> GetProductVersionPlatformByProductVerisonIDAsync(Guid product_version_id, int skip = 0, int take = 10, string order_by = "", bool descending = false)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "productversionplatforms/by_productverisonid/{product_version_id}";
            request.AddUrlSegment("product_version_id", product_version_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            
            return this.Sdk.ExecuteAsync<ListResult<ProductVersionPlatform>>(request);
        }
        
        public Task<ListResult<ProductVersionPlatform>> GetProductVersionPlatformByPlatformIDAsync(Guid platform_id, int skip = 0, int take = 10, string order_by = "", bool descending = false)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "productversionplatforms/by_platformid/{platform_id}";
            request.AddUrlSegment("platform_id", platform_id.ToString());
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            
            return this.Sdk.ExecuteAsync<ListResult<ProductVersionPlatform>>(request);
        }
        

        public Task<ItemResult<ProductVersionPlatform>> CreateProductVersionPlatformAsync(ProductVersionPlatform productversionplatform)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "productversionplatforms";
            request.AddJsonBody(productversionplatform);
            return this.Sdk.ExecuteAsync<ItemResult<ProductVersionPlatform>>(request);
        }

        public Task<ItemResult<ProductVersionPlatform>> UpdateProductVersionPlatformAsync(Guid product_version_platform_id, ProductVersionPlatform productversionplatform)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "productversionplatforms/{product_version_platform_id}";
            request.AddUrlSegment("product_version_platform_id", product_version_platform_id.ToString());
            request.AddJsonBody(productversionplatform);
            return this.Sdk.ExecuteAsync<ItemResult<ProductVersionPlatform>>(request);
        }

        

        public Task<ActionResult> DeleteProductVersionPlatformAsync(Guid product_version_platform_id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "productversionplatforms/{product_version_platform_id}";
            request.AddUrlSegment("product_version_platform_id", product_version_platform_id.ToString());
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
