//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using am = AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stencil.Data.Sql;
using Stencil.Domain;

namespace Stencil.Primary.Mapping
{
    public partial class PrimaryMappingProfile : AutoMapper.Profile
    {
        public PrimaryMappingProfile()
            : base("PrimaryMappingProfile")
        {
        }

        protected override void Configure()
        {
            this.DbAndDomainMappings();
            this.DomainAndSDKMappings();
            
            this.DbAndDomainMappings_Manual();
            this.DomainAndSDKMappings_Manual();
        }
        
        partial void DbAndDomainMappings_Manual();
        partial void DomainAndSDKMappings_Manual();
        
        protected void DbAndDomainMappings()
        {
            am.Mapper.CreateMap<DateTimeOffset?, DateTime?>()
                .ConvertUsing(x => x.HasValue ? x.Value.UtcDateTime : (DateTime?)null);

            am.Mapper.CreateMap<DateTimeOffset, DateTime?>()
                .ConvertUsing(x => x.UtcDateTime);

            am.Mapper.CreateMap<DateTimeOffset, DateTime>()
                .ConvertUsing(x => x.UtcDateTime);

            am.Mapper.CreateMap<DateTime?, DateTimeOffset?>()
                .ConvertUsing(x => x.HasValue ? new DateTimeOffset(x.Value) : (DateTimeOffset?)null);
                
            am.Mapper.CreateMap<dbGlobalSetting, GlobalSetting>();
            am.Mapper.CreateMap<GlobalSetting, dbGlobalSetting>();
            am.Mapper.CreateMap<dbAccount, Account>();
            am.Mapper.CreateMap<Account, dbAccount>();
            am.Mapper.CreateMap<dbProduct, Product>();
            am.Mapper.CreateMap<Product, dbProduct>();
            am.Mapper.CreateMap<dbPlatform, Platform>();
            am.Mapper.CreateMap<Platform, dbPlatform>();
            am.Mapper.CreateMap<dbProductVersion, ProductVersion>();
            am.Mapper.CreateMap<ProductVersion, dbProductVersion>();
            am.Mapper.CreateMap<dbAsset, Asset>();
            am.Mapper.CreateMap<Asset, dbAsset>();
            
        }
        protected void DomainAndSDKMappings()
        {
            am.Mapper.CreateMap<Domain.AssetType, SDK.Models.AssetType>().ConvertUsing(x => (SDK.Models.AssetType)(int)x);
            am.Mapper.CreateMap<SDK.Models.AssetType, Domain.AssetType>().ConvertUsing(x => (Domain.AssetType)(int)x);
            am.Mapper.CreateMap<Domain.Dependency, SDK.Models.Dependency>().ConvertUsing(x => (SDK.Models.Dependency)(int)x);
            am.Mapper.CreateMap<SDK.Models.Dependency, Domain.Dependency>().ConvertUsing(x => (Domain.Dependency)(int)x);
            am.Mapper.CreateMap<Domain.Bitness, SDK.Models.Bitness>().ConvertUsing(x => (SDK.Models.Bitness)(int)x);
            am.Mapper.CreateMap<SDK.Models.Bitness, Domain.Bitness>().ConvertUsing(x => (Domain.Bitness)(int)x);
            
            am.Mapper.CreateMap<Domain.GlobalSetting, SDK.Models.GlobalSetting>();
            am.Mapper.CreateMap<SDK.Models.GlobalSetting, Domain.GlobalSetting>();
            
            am.Mapper.CreateMap<Domain.Account, SDK.Models.Account>();
            am.Mapper.CreateMap<SDK.Models.Account, Domain.Account>();
            
            am.Mapper.CreateMap<Domain.Product, SDK.Models.Product>();
            am.Mapper.CreateMap<SDK.Models.Product, Domain.Product>();
            
            am.Mapper.CreateMap<Domain.Platform, SDK.Models.Platform>();
            am.Mapper.CreateMap<SDK.Models.Platform, Domain.Platform>();
            
            am.Mapper.CreateMap<Domain.ProductVersion, SDK.Models.ProductVersion>();
            am.Mapper.CreateMap<SDK.Models.ProductVersion, Domain.ProductVersion>();
            
            am.Mapper.CreateMap<Domain.Asset, SDK.Models.Asset>();
            am.Mapper.CreateMap<SDK.Models.Asset, Domain.Asset>();
            
        }
    }
}

