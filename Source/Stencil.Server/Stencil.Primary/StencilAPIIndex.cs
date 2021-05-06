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
using System.Threading.Tasks;
using Stencil.Primary.Business.Index;

namespace Stencil.Primary
{
    public class StencilAPIIndex : BaseClass
    {
        public StencilAPIIndex(IFoundation ifoundation)
            : base(ifoundation)
        {
        }

        public IAccountIndex Accounts
        {
            get { return this.IFoundation.Resolve<IAccountIndex>(); }
        }
        public IProductIndex Products
        {
            get { return this.IFoundation.Resolve<IProductIndex>(); }
        }
        public IPlatformIndex Platforms
        {
            get { return this.IFoundation.Resolve<IPlatformIndex>(); }
        }
        public IProductVersionIndex ProductVersions
        {
            get { return this.IFoundation.Resolve<IProductVersionIndex>(); }
        }
        
    }
}


