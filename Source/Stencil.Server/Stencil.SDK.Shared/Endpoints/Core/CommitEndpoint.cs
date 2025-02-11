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
    public partial class CommitEndpoint : EndpointBase
    {
        public CommitEndpoint(StencilSDK api)
            : base(api)
        {

        }
        
        public Task<ItemResult<Commit>> GetCommitAsync(Guid commit_id)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "commits/{commit_id}";
            request.AddUrlSegment("commit_id", commit_id.ToString());
            
            return this.Sdk.ExecuteAsync<ItemResult<Commit>>(request);
        }
        
        

        public Task<ItemResult<Commit>> CreateCommitAsync(Commit commit)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "commits";
            request.AddJsonBody(commit);
            return this.Sdk.ExecuteAsync<ItemResult<Commit>>(request);
        }

        public Task<ItemResult<Commit>> UpdateCommitAsync(Guid commit_id, Commit commit)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "commits/{commit_id}";
            request.AddUrlSegment("commit_id", commit_id.ToString());
            request.AddJsonBody(commit);
            return this.Sdk.ExecuteAsync<ItemResult<Commit>>(request);
        }

        

        public Task<ActionResult> DeleteCommitAsync(Guid commit_id)
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "commits/{commit_id}";
            request.AddUrlSegment("commit_id", commit_id.ToString());
            return this.Sdk.ExecuteAsync<ActionResult>(request);
        }
    }
}
