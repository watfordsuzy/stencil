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

namespace Stencil.Primary
{
    public static partial class _DomainModelExtensions
    {
        
        public static dbGlobalSetting ToDbModel(this GlobalSetting entity, dbGlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbGlobalSetting(); }
                return am.Mapper.Map<GlobalSetting, dbGlobalSetting>(entity, destination);
            }
            return null;
        }
        public static GlobalSetting ToDomainModel(this dbGlobalSetting entity, GlobalSetting destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new GlobalSetting(); }
                return am.Mapper.Map<dbGlobalSetting, GlobalSetting>(entity, destination);
            }
            return null;
        }
        public static List<GlobalSetting> ToDomainModel(this IEnumerable<dbGlobalSetting> entities)
        {
            List<GlobalSetting> result = new List<GlobalSetting>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbAccount ToDbModel(this Account entity, dbAccount destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbAccount(); }
                return am.Mapper.Map<Account, dbAccount>(entity, destination);
            }
            return null;
        }
        public static Account ToDomainModel(this dbAccount entity, Account destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Account(); }
                return am.Mapper.Map<dbAccount, Account>(entity, destination);
            }
            return null;
        }
        public static List<Account> ToDomainModel(this IEnumerable<dbAccount> entities)
        {
            List<Account> result = new List<Account>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbProduct ToDbModel(this Product entity, dbProduct destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbProduct(); }
                return am.Mapper.Map<Product, dbProduct>(entity, destination);
            }
            return null;
        }
        public static Product ToDomainModel(this dbProduct entity, Product destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Product(); }
                return am.Mapper.Map<dbProduct, Product>(entity, destination);
            }
            return null;
        }
        public static List<Product> ToDomainModel(this IEnumerable<dbProduct> entities)
        {
            List<Product> result = new List<Product>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbPlatform ToDbModel(this Platform entity, dbPlatform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbPlatform(); }
                return am.Mapper.Map<Platform, dbPlatform>(entity, destination);
            }
            return null;
        }
        public static Platform ToDomainModel(this dbPlatform entity, Platform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Platform(); }
                return am.Mapper.Map<dbPlatform, Platform>(entity, destination);
            }
            return null;
        }
        public static List<Platform> ToDomainModel(this IEnumerable<dbPlatform> entities)
        {
            List<Platform> result = new List<Platform>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbProductVersion ToDbModel(this ProductVersion entity, dbProductVersion destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbProductVersion(); }
                return am.Mapper.Map<ProductVersion, dbProductVersion>(entity, destination);
            }
            return null;
        }
        public static ProductVersion ToDomainModel(this dbProductVersion entity, ProductVersion destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new ProductVersion(); }
                return am.Mapper.Map<dbProductVersion, ProductVersion>(entity, destination);
            }
            return null;
        }
        public static List<ProductVersion> ToDomainModel(this IEnumerable<dbProductVersion> entities)
        {
            List<ProductVersion> result = new List<ProductVersion>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbProductVersionPlatform ToDbModel(this ProductVersionPlatform entity, dbProductVersionPlatform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbProductVersionPlatform(); }
                return am.Mapper.Map<ProductVersionPlatform, dbProductVersionPlatform>(entity, destination);
            }
            return null;
        }
        public static ProductVersionPlatform ToDomainModel(this dbProductVersionPlatform entity, ProductVersionPlatform destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new ProductVersionPlatform(); }
                return am.Mapper.Map<dbProductVersionPlatform, ProductVersionPlatform>(entity, destination);
            }
            return null;
        }
        public static List<ProductVersionPlatform> ToDomainModel(this IEnumerable<dbProductVersionPlatform> entities)
        {
            List<ProductVersionPlatform> result = new List<ProductVersionPlatform>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbTicket ToDbModel(this Ticket entity, dbTicket destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbTicket(); }
                return am.Mapper.Map<Ticket, dbTicket>(entity, destination);
            }
            return null;
        }
        public static Ticket ToDomainModel(this dbTicket entity, Ticket destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Ticket(); }
                return am.Mapper.Map<dbTicket, Ticket>(entity, destination);
            }
            return null;
        }
        public static List<Ticket> ToDomainModel(this IEnumerable<dbTicket> entities)
        {
            List<Ticket> result = new List<Ticket>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbAffectedProduct ToDbModel(this AffectedProduct entity, dbAffectedProduct destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbAffectedProduct(); }
                return am.Mapper.Map<AffectedProduct, dbAffectedProduct>(entity, destination);
            }
            return null;
        }
        public static AffectedProduct ToDomainModel(this dbAffectedProduct entity, AffectedProduct destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new AffectedProduct(); }
                return am.Mapper.Map<dbAffectedProduct, AffectedProduct>(entity, destination);
            }
            return null;
        }
        public static List<AffectedProduct> ToDomainModel(this IEnumerable<dbAffectedProduct> entities)
        {
            List<AffectedProduct> result = new List<AffectedProduct>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
        public static dbAsset ToDbModel(this Asset entity, dbAsset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dbAsset(); }
                return am.Mapper.Map<Asset, dbAsset>(entity, destination);
            }
            return null;
        }
        public static Asset ToDomainModel(this dbAsset entity, Asset destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Asset(); }
                return am.Mapper.Map<dbAsset, Asset>(entity, destination);
            }
            return null;
        }
        public static List<Asset> ToDomainModel(this IEnumerable<dbAsset> entities)
        {
            List<Asset> result = new List<Asset>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        
    }
}

