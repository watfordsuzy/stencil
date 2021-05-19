//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stencil.Primary.Business.Direct;

namespace Stencil.Primary
{
    public class StencilAPIDirect : BaseClass
    {
        public StencilAPIDirect(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        public IGlobalSettingBusiness GlobalSettings
        {
            get { return this.IFoundation.Resolve<IGlobalSettingBusiness>(); }
        }
        public IAccountBusiness Accounts
        {
            get { return this.IFoundation.Resolve<IAccountBusiness>(); }
        }
        public IProductBusiness Products
        {
            get { return this.IFoundation.Resolve<IProductBusiness>(); }
        }
        public IPlatformBusiness Platforms
        {
            get { return this.IFoundation.Resolve<IPlatformBusiness>(); }
        }
        public IProductVersionBusiness ProductVersions
        {
            get { return this.IFoundation.Resolve<IProductVersionBusiness>(); }
        }
        public IProductVersionPlatformBusiness ProductVersionPlatforms
        {
            get { return this.IFoundation.Resolve<IProductVersionPlatformBusiness>(); }
        }
        public ITicketBusiness Tickets
        {
            get { return this.IFoundation.Resolve<ITicketBusiness>(); }
        }
        public IAffectedProductBusiness AffectedProducts
        {
            get { return this.IFoundation.Resolve<IAffectedProductBusiness>(); }
        }
        public ICommitBusiness Commits
        {
            get { return this.IFoundation.Resolve<ICommitBusiness>(); }
        }
        public ITicketCommentBusiness TicketComments
        {
            get { return this.IFoundation.Resolve<ITicketCommentBusiness>(); }
        }
        public IAssetBusiness Assets
        {
            get { return this.IFoundation.Resolve<IAssetBusiness>(); }
        }
        
    }
}


