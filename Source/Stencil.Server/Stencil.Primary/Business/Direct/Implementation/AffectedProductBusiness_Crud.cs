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
    public partial class AffectedProductBusiness : BusinessBase, IAffectedProductBusiness
    {
        public AffectedProductBusiness(IFoundation foundation)
            : base(foundation, "AffectedProduct")
        {
        }
        
        protected IAffectedProductSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IAffectedProductSynchronizer>();
            }
        }

        public AffectedProduct Insert(AffectedProduct insertAffectedProduct)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    

                    this.PreProcess(insertAffectedProduct, true);
                    var interception = this.Intercept(insertAffectedProduct, true);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertAffectedProduct.affected_product_id == Guid.Empty)
                    {
                        insertAffectedProduct.affected_product_id = Guid.NewGuid();
                    }
                    insertAffectedProduct.created_utc = DateTime.UtcNow;
                    insertAffectedProduct.updated_utc = insertAffectedProduct.created_utc;

                    dbAffectedProduct dbModel = insertAffectedProduct.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    db.dbAffectedProducts.Add(dbModel);

                    db.SaveChanges();
                    
                    this.AfterInsertPersisted(db, dbModel);
                    
                    this.Synchronizer.SynchronizeItem(dbModel.affected_product_id, Availability.Retrievable);
                    this.AfterInsertIndexed(db, dbModel);
                    
                    this.DependencyCoordinator.AffectedProductInvalidated(Dependency.None, dbModel.affected_product_id);
                }
                return this.GetById(insertAffectedProduct.affected_product_id);
            });
        }
        public AffectedProduct Update(AffectedProduct updateAffectedProduct)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    this.PreProcess(updateAffectedProduct, false);
                    var interception = this.Intercept(updateAffectedProduct, false);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateAffectedProduct.updated_utc = DateTime.UtcNow;
                    
                    dbAffectedProduct found = (from n in db.dbAffectedProducts
                                    where n.affected_product_id == updateAffectedProduct.affected_product_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        AffectedProduct previous = found.ToDomainModel();
                        
                        found = updateAffectedProduct.ToDbModel(found);
                        found.InvalidateSync(this.DefaultAgent, "updated");
                        db.SaveChanges();
                        
                        this.AfterUpdatePersisted(db, found, previous);
                        
                        this.Synchronizer.SynchronizeItem(found.affected_product_id, Availability.Retrievable);
                        this.AfterUpdateIndexed(db, found);
                        
                        this.DependencyCoordinator.AffectedProductInvalidated(Dependency.None, found.affected_product_id);
                    
                    }
                    
                    return this.GetById(updateAffectedProduct.affected_product_id);
                }
            });
        }
        public void Delete(Guid affected_product_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var db = base.CreateSQLContext())
                {
                    dbAffectedProduct found = (from a in db.dbAffectedProducts
                                    where a.affected_product_id == affected_product_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");
                        db.SaveChanges();
                        
                        this.AfterDeletePersisted(db, found);
                        
                        this.Synchronizer.SynchronizeItem(found.affected_product_id, Availability.Retrievable);
                        
                        this.DependencyCoordinator.AffectedProductInvalidated(Dependency.None, found.affected_product_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid affected_product_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spAffectedProduct_SyncUpdate(affected_product_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spAffectedProduct_SyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        public void SynchronizationHydrateUpdate(Guid affected_product_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spAffectedProduct_HydrateSyncUpdate(affected_product_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spAffectedProduct_HydrateSyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        
        public AffectedProduct GetById(Guid affected_product_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    dbAffectedProduct result = (from n in db.dbAffectedProducts
                                     where (n.affected_product_id == affected_product_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<AffectedProduct> GetByTicketID(Guid ticket_id)
        {
            return base.ExecuteFunction("GetByTicketID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbAffectedProducts
                                     where (n.ticket_id == ticket_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        public List<AffectedProduct> GetByProductID(Guid product_id)
        {
            return base.ExecuteFunction("GetByProductID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbAffectedProducts
                                     where (n.product_id == product_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void Invalidate(Guid affected_product_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbAffectedProducts
                        .Where(x => x.affected_product_id == affected_product_id)
                        .Update(x => new dbAffectedProduct() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        


        
        
        public InterceptArgs<AffectedProduct> Intercept(AffectedProduct affectedproduct, bool forInsert)
        {
            InterceptArgs<AffectedProduct> args = new InterceptArgs<AffectedProduct>()
            {
                ForInsert = forInsert,
                ReturnEntity = affectedproduct
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void PerformIntercept(InterceptArgs<AffectedProduct> args);
        partial void PreProcess(AffectedProduct affectedproduct, bool forInsert);
        partial void AfterInsertPersisted(StencilContext db, dbAffectedProduct affectedproduct);
        partial void AfterUpdatePersisted(StencilContext db, dbAffectedProduct affectedproduct, AffectedProduct previous);
        partial void AfterDeletePersisted(StencilContext db, dbAffectedProduct affectedproduct);
        partial void AfterUpdateIndexed(StencilContext db, dbAffectedProduct affectedproduct);
        partial void AfterInsertIndexed(StencilContext db, dbAffectedProduct affectedproduct);
    }
}

