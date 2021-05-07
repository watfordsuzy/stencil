//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using am = AutoMapper;
using Codeable.Foundation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stencil.Data.Sql;
using Stencil.Domain;

namespace Stencil.Primary
{
    public static partial class _DomainModelExtensions
    {
        
        public static GlobalSetting ToDomainModel(this SDK.Models.GlobalSetting entity, GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.GlobalSetting(); }
                GlobalSetting result = am.Mapper.Map<SDK.Models.GlobalSetting, GlobalSetting>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.GlobalSetting ToSDKModel(this GlobalSetting entity, SDK.Models.GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.GlobalSetting(); }
                SDK.Models.GlobalSetting result = am.Mapper.Map<GlobalSetting, SDK.Models.GlobalSetting>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.GlobalSetting> ToSDKModel(this IEnumerable<GlobalSetting> entities)
        {
            List<SDK.Models.GlobalSetting> result = new List<SDK.Models.GlobalSetting>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Account ToDomainModel(this SDK.Models.Account entity, Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Account(); }
                Account result = am.Mapper.Map<SDK.Models.Account, Account>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Account ToSDKModel(this Account entity, SDK.Models.Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Account(); }
                SDK.Models.Account result = am.Mapper.Map<Account, SDK.Models.Account>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Account> ToSDKModel(this IEnumerable<Account> entities)
        {
            List<SDK.Models.Account> result = new List<SDK.Models.Account>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Product ToDomainModel(this SDK.Models.Product entity, Product destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Product(); }
                Product result = am.Mapper.Map<SDK.Models.Product, Product>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Product ToSDKModel(this Product entity, SDK.Models.Product destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Product(); }
                SDK.Models.Product result = am.Mapper.Map<Product, SDK.Models.Product>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Product> ToSDKModel(this IEnumerable<Product> entities)
        {
            List<SDK.Models.Product> result = new List<SDK.Models.Product>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Platform ToDomainModel(this SDK.Models.Platform entity, Platform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Platform(); }
                Platform result = am.Mapper.Map<SDK.Models.Platform, Platform>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Platform ToSDKModel(this Platform entity, SDK.Models.Platform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Platform(); }
                SDK.Models.Platform result = am.Mapper.Map<Platform, SDK.Models.Platform>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Platform> ToSDKModel(this IEnumerable<Platform> entities)
        {
            List<SDK.Models.Platform> result = new List<SDK.Models.Platform>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static ProductVersion ToDomainModel(this SDK.Models.ProductVersion entity, ProductVersion destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.ProductVersion(); }
                ProductVersion result = am.Mapper.Map<SDK.Models.ProductVersion, ProductVersion>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.ProductVersion ToSDKModel(this ProductVersion entity, SDK.Models.ProductVersion destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.ProductVersion(); }
                SDK.Models.ProductVersion result = am.Mapper.Map<ProductVersion, SDK.Models.ProductVersion>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.ProductVersion> ToSDKModel(this IEnumerable<ProductVersion> entities)
        {
            List<SDK.Models.ProductVersion> result = new List<SDK.Models.ProductVersion>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static ProductVersionPlatform ToDomainModel(this SDK.Models.ProductVersionPlatform entity, ProductVersionPlatform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.ProductVersionPlatform(); }
                ProductVersionPlatform result = am.Mapper.Map<SDK.Models.ProductVersionPlatform, ProductVersionPlatform>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.ProductVersionPlatform ToSDKModel(this ProductVersionPlatform entity, SDK.Models.ProductVersionPlatform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.ProductVersionPlatform(); }
                SDK.Models.ProductVersionPlatform result = am.Mapper.Map<ProductVersionPlatform, SDK.Models.ProductVersionPlatform>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.ProductVersionPlatform> ToSDKModel(this IEnumerable<ProductVersionPlatform> entities)
        {
            List<SDK.Models.ProductVersionPlatform> result = new List<SDK.Models.ProductVersionPlatform>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Ticket ToDomainModel(this SDK.Models.Ticket entity, Ticket destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Ticket(); }
                Ticket result = am.Mapper.Map<SDK.Models.Ticket, Ticket>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Ticket ToSDKModel(this Ticket entity, SDK.Models.Ticket destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Ticket(); }
                SDK.Models.Ticket result = am.Mapper.Map<Ticket, SDK.Models.Ticket>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Ticket> ToSDKModel(this IEnumerable<Ticket> entities)
        {
            List<SDK.Models.Ticket> result = new List<SDK.Models.Ticket>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
        public static Asset ToDomainModel(this SDK.Models.Asset entity, Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.Asset(); }
                Asset result = am.Mapper.Map<SDK.Models.Asset, Asset>(entity, destination);
                return result;
            }
            return null;
        }
        public static SDK.Models.Asset ToSDKModel(this Asset entity, SDK.Models.Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.Asset(); }
                SDK.Models.Asset result = am.Mapper.Map<Asset, SDK.Models.Asset>(entity, destination);
                return result;
            }
            return null;
        }
        public static List<SDK.Models.Asset> ToSDKModel(this IEnumerable<Asset> entities)
        {
            List<SDK.Models.Asset> result = new List<SDK.Models.Asset>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        
    }
}

