//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stencil.Domain;

namespace Stencil.Primary.Business.Integration
{
    public interface IDependencyCoordinator
    {
        void GlobalSettingInvalidated(Dependency affectedDependencies, Guid global_setting_id);
        void AccountInvalidated(Dependency affectedDependencies, Guid account_id);
        void AssetInvalidated(Dependency affectedDependencies, Guid asset_id);
        
    }
}


