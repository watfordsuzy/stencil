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
    public partial class ProductVersionPlatformBusiness : BusinessBase, IProductVersionPlatformBusiness
    {
        public ProductVersionPlatformBusiness(IFoundation foundation)
            : base(foundation, "ProductVersionPlatform")
        {
        }
        
        protected IProductVersionPlatformSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IProductVersionPlatformSynchronizer>();
            }
        }

        public ProductVersionPlatform Insert(ProductVersionPlatform insertProductVersionPlatform)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    

                    this.PreProcess(insertProductVersionPlatform, true);
                    var interception = this.Intercept(insertProductVersionPlatform, true);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertProductVersionPlatform.product_version_platform_id == Guid.Empty)
                    {
                        insertProductVersionPlatform.product_version_platform_id = Guid.NewGuid();
                    }
                    insertProductVersionPlatform.created_utc = DateTime.UtcNow;
                    insertProductVersionPlatform.updated_utc = insertProductVersionPlatform.created_utc;

                    dbProductVersionPlatform dbModel = insertProductVersionPlatform.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    db.dbProductVersionPlatforms.Add(dbModel);

                    db.SaveChanges();
                    
                    this.AfterInsertPersisted(db, dbModel);
                    
                    this.Synchronizer.SynchronizeItem(dbModel.product_version_platform_id, Availability.Retrievable);
                    this.AfterInsertIndexed(db, dbModel);
                    
                    this.DependencyCoordinator.ProductVersionPlatformInvalidated(Dependency.None, dbModel.product_version_platform_id);
                }
                return this.GetById(insertProductVersionPlatform.product_version_platform_id);
            });
        }
        public ProductVersionPlatform Update(ProductVersionPlatform updateProductVersionPlatform)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    this.PreProcess(updateProductVersionPlatform, false);
                    var interception = this.Intercept(updateProductVersionPlatform, false);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateProductVersionPlatform.updated_utc = DateTime.UtcNow;
                    
                    dbProductVersionPlatform found = (from n in db.dbProductVersionPlatforms
                                    where n.product_version_platform_id == updateProductVersionPlatform.product_version_platform_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        ProductVersionPlatform previous = found.ToDomainModel();
                        
                        found = updateProductVersionPlatform.ToDbModel(found);

                        this.BeforeUpdatePersisted(found, previous);

                        found.InvalidateSync(this.DefaultAgent, "updated");
                        db.SaveChanges();
                        
                        this.AfterUpdatePersisted(db, found, previous);
                        
                        this.Synchronizer.SynchronizeItem(found.product_version_platform_id, Availability.Retrievable);
                        this.AfterUpdateIndexed(db, found);
                        
                        this.DependencyCoordinator.ProductVersionPlatformInvalidated(Dependency.None, found.product_version_platform_id);
                    
                    }
                    
                    return this.GetById(updateProductVersionPlatform.product_version_platform_id);
                }
            });
        }
        public void Delete(Guid product_version_platform_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var db = base.CreateSQLContext())
                {
                    dbProductVersionPlatform found = (from a in db.dbProductVersionPlatforms
                                    where a.product_version_platform_id == product_version_platform_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");
                        db.SaveChanges();
                        
                        this.AfterDeletePersisted(db, found);
                        
                        this.Synchronizer.SynchronizeItem(found.product_version_platform_id, Availability.Retrievable);
                        
                        this.DependencyCoordinator.ProductVersionPlatformInvalidated(Dependency.None, found.product_version_platform_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid product_version_platform_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spProductVersionPlatform_SyncUpdate(product_version_platform_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spProductVersionPlatform_SyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        public void SynchronizationHydrateUpdate(Guid product_version_platform_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spProductVersionPlatform_HydrateSyncUpdate(product_version_platform_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spProductVersionPlatform_HydrateSyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        
        public ProductVersionPlatform GetById(Guid product_version_platform_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    dbProductVersionPlatform result = (from n in db.dbProductVersionPlatforms
                                     where (n.product_version_platform_id == product_version_platform_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<ProductVersionPlatform> GetByProductVerisonID(Guid product_version_id)
        {
            return base.ExecuteFunction("GetByProductVerisonID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbProductVersionPlatforms
                                     where (n.product_version_id == product_version_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void InvalidateForProductVerisonID(Guid product_version_id, string reason)
        {
            base.ExecuteMethod("InvalidateForProductVerisonID", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbProductVersionPlatforms
                        .Where(x => x.product_version_id == product_version_id)
                        .Update(x => new dbProductVersionPlatform() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                    
                }
            });
        }
        public List<ProductVersionPlatform> GetByPlatformID(Guid platform_id)
        {
            return base.ExecuteFunction("GetByPlatformID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbProductVersionPlatforms
                                     where (n.platform_id == platform_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void InvalidateForPlatformID(Guid platform_id, string reason)
        {
            base.ExecuteMethod("InvalidateForPlatformID", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbProductVersionPlatforms
                        .Where(x => x.platform_id == platform_id)
                        .Update(x => new dbProductVersionPlatform() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                    
                }
            });
        }
        
        public void Invalidate(Guid product_version_platform_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbProductVersionPlatforms
                        .Where(x => x.product_version_platform_id == product_version_platform_id)
                        .Update(x => new dbProductVersionPlatform() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        


        
        
        public InterceptArgs<ProductVersionPlatform> Intercept(ProductVersionPlatform productversionplatform, bool forInsert)
        {
            InterceptArgs<ProductVersionPlatform> args = new InterceptArgs<ProductVersionPlatform>()
            {
                ForInsert = forInsert,
                ReturnEntity = productversionplatform
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void PerformIntercept(InterceptArgs<ProductVersionPlatform> args);
        partial void PreProcess(ProductVersionPlatform productversionplatform, bool forInsert);
        partial void AfterInsertPersisted(StencilContext db, dbProductVersionPlatform productversionplatform);
        partial void BeforeUpdatePersisted(dbProductVersionPlatform productversionplatform, ProductVersionPlatform previous);
        partial void AfterUpdatePersisted(StencilContext db, dbProductVersionPlatform productversionplatform, ProductVersionPlatform previous);
        partial void AfterDeletePersisted(StencilContext db, dbProductVersionPlatform productversionplatform);
        partial void AfterUpdateIndexed(StencilContext db, dbProductVersionPlatform productversionplatform);
        partial void AfterInsertIndexed(StencilContext db, dbProductVersionPlatform productversionplatform);
    }
}

