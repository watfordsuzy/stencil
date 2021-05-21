//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Codeable.Foundation.Common;
using Codeable.Foundation.Common.Aspect;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stencil.Domain;
using Stencil.Data.Sql;
using Stencil.Primary.Synchronization;

namespace Stencil.Primary.Business.Direct.Implementation
{
    // WARNING: THIS FILE IS GENERATED
    public partial class ProductVersionBusiness : BusinessBase, IProductVersionBusiness
    {
        public ProductVersionBusiness(IFoundation foundation)
            : base(foundation, "ProductVersion")
        {
        }
        
        protected IProductVersionSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IProductVersionSynchronizer>();
            }
        }

        public ProductVersion Insert(ProductVersion insertProductVersion)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    

                    this.PreProcess(insertProductVersion, true);
                    var interception = this.Intercept(insertProductVersion, true);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertProductVersion.product_version_id == Guid.Empty)
                    {
                        insertProductVersion.product_version_id = Guid.NewGuid();
                    }
                    insertProductVersion.created_utc = DateTime.UtcNow;
                    insertProductVersion.updated_utc = insertProductVersion.created_utc;

                    dbProductVersion dbModel = insertProductVersion.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    db.dbProductVersions.Add(dbModel);

                    db.SaveChanges();
                    
                    this.AfterInsertPersisted(db, dbModel);
                    
                    this.Synchronizer.SynchronizeItem(dbModel.product_version_id, Availability.Retrievable);
                    this.AfterInsertIndexed(db, dbModel);
                    
                    this.DependencyCoordinator.ProductVersionInvalidated(Dependency.None, dbModel.product_version_id);
                }
                return this.GetById(insertProductVersion.product_version_id);
            });
        }
        public ProductVersion Update(ProductVersion updateProductVersion)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    this.PreProcess(updateProductVersion, false);
                    var interception = this.Intercept(updateProductVersion, false);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateProductVersion.updated_utc = DateTime.UtcNow;
                    
                    dbProductVersion found = (from n in db.dbProductVersions
                                    where n.product_version_id == updateProductVersion.product_version_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        ProductVersion previous = found.ToDomainModel();
                        
                        found = updateProductVersion.ToDbModel(found);

                        this.BeforeUpdatePersisted(found, previous);

                        found.InvalidateSync(this.DefaultAgent, "updated");
                        db.SaveChanges();
                        
                        this.AfterUpdatePersisted(db, found, previous);
                        
                        this.Synchronizer.SynchronizeItem(found.product_version_id, Availability.Retrievable);
                        this.AfterUpdateIndexed(db, found);
                        
                        this.DependencyCoordinator.ProductVersionInvalidated(Dependency.None, found.product_version_id);
                    
                    }
                    
                    return this.GetById(updateProductVersion.product_version_id);
                }
            });
        }
        public void Delete(Guid product_version_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var db = base.CreateSQLContext())
                {
                    dbProductVersion found = (from a in db.dbProductVersions
                                    where a.product_version_id == product_version_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");
                        db.SaveChanges();
                        
                        this.AfterDeletePersisted(db, found);
                        
                        this.Synchronizer.SynchronizeItem(found.product_version_id, Availability.Retrievable);
                        
                        this.DependencyCoordinator.ProductVersionInvalidated(Dependency.None, found.product_version_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid product_version_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spProductVersion_SyncUpdate(product_version_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spProductVersion_SyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        public void SynchronizationHydrateUpdate(Guid product_version_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spProductVersion_HydrateSyncUpdate(product_version_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spProductVersion_HydrateSyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        
        public ProductVersion GetById(Guid product_version_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    dbProductVersion result = (from n in db.dbProductVersions
                                     where (n.product_version_id == product_version_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<ProductVersion> GetByProductID(Guid product_id)
        {
            return base.ExecuteFunction("GetByProductID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbProductVersions
                                     where (n.product_id == product_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid product_version_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbProductVersions
                        .Where(x => x.product_version_id == product_version_id)
                        .Update(x => new dbProductVersion() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        public List<ProductVersion> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.dbProductVersions
                                where (keyword == "" 
                                    || p.version.Contains(keyword)
                                )
                                select p);

                    List<dbProductVersion> result = new List<dbProductVersion>();

                    switch (order_by)
                    {
                        case "version":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.version).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.version).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        case "release_date_utc":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.release_date_utc).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.release_date_utc).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        case "end_of_life_date_utc":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.end_of_life_date_utc).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.end_of_life_date_utc).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        default:
                            result = data.OrderBy(s => s.product_version_id).Skip(skip).Take(take).ToList();
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        


        
        
        public InterceptArgs<ProductVersion> Intercept(ProductVersion productversion, bool forInsert)
        {
            InterceptArgs<ProductVersion> args = new InterceptArgs<ProductVersion>()
            {
                ForInsert = forInsert,
                ReturnEntity = productversion
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void PerformIntercept(InterceptArgs<ProductVersion> args);
        partial void PreProcess(ProductVersion productversion, bool forInsert);
        partial void AfterInsertPersisted(StencilContext db, dbProductVersion productversion);
        partial void BeforeUpdatePersisted(dbProductVersion productversion, ProductVersion previous);
        partial void AfterUpdatePersisted(StencilContext db, dbProductVersion productversion, ProductVersion previous);
        partial void AfterDeletePersisted(StencilContext db, dbProductVersion productversion);
        partial void AfterUpdateIndexed(StencilContext db, dbProductVersion productversion);
        partial void AfterInsertIndexed(StencilContext db, dbProductVersion productversion);
    }
}

