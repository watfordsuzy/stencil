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
    public partial class PlatformBusiness : BusinessBase, IPlatformBusiness
    {
        public PlatformBusiness(IFoundation foundation)
            : base(foundation, "Platform")
        {
        }
        
        protected IPlatformSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<IPlatformSynchronizer>();
            }
        }

        public Platform Insert(Platform insertPlatform)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    

                    this.PreProcess(insertPlatform, true);
                    var interception = this.Intercept(insertPlatform, true);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertPlatform.platform_id == Guid.Empty)
                    {
                        insertPlatform.platform_id = Guid.NewGuid();
                    }
                    insertPlatform.created_utc = DateTime.UtcNow;
                    insertPlatform.updated_utc = insertPlatform.created_utc;

                    dbPlatform dbModel = insertPlatform.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    db.dbPlatforms.Add(dbModel);

                    db.SaveChanges();
                    
                    this.AfterInsertPersisted(db, dbModel);
                    
                    this.Synchronizer.SynchronizeItem(dbModel.platform_id, Availability.Retrievable);
                    this.AfterInsertIndexed(db, dbModel);
                    
                    this.DependencyCoordinator.PlatformInvalidated(Dependency.None, dbModel.platform_id);
                }
                return this.GetById(insertPlatform.platform_id);
            });
        }
        public Platform Update(Platform updatePlatform)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    this.PreProcess(updatePlatform, false);
                    var interception = this.Intercept(updatePlatform, false);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updatePlatform.updated_utc = DateTime.UtcNow;
                    
                    dbPlatform found = (from n in db.dbPlatforms
                                    where n.platform_id == updatePlatform.platform_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Platform previous = found.ToDomainModel();
                        
                        found = updatePlatform.ToDbModel(found);

                        this.BeforeUpdatePersisted(found, previous);

                        found.InvalidateSync(this.DefaultAgent, "updated");
                        db.SaveChanges();
                        
                        this.AfterUpdatePersisted(db, found, previous);
                        
                        this.Synchronizer.SynchronizeItem(found.platform_id, Availability.Retrievable);
                        this.AfterUpdateIndexed(db, found);
                        
                        this.DependencyCoordinator.PlatformInvalidated(Dependency.None, found.platform_id);
                    
                    }
                    
                    return this.GetById(updatePlatform.platform_id);
                }
            });
        }
        public void Delete(Guid platform_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var db = base.CreateSQLContext())
                {
                    dbPlatform found = (from a in db.dbPlatforms
                                    where a.platform_id == platform_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");
                        db.SaveChanges();
                        
                        this.AfterDeletePersisted(db, found);
                        
                        this.Synchronizer.SynchronizeItem(found.platform_id, Availability.Retrievable);
                        
                        this.DependencyCoordinator.PlatformInvalidated(Dependency.None, found.platform_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid platform_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spPlatform_SyncUpdate(platform_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spPlatform_SyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        public void SynchronizationHydrateUpdate(Guid platform_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spPlatform_HydrateSyncUpdate(platform_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spPlatform_HydrateSyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        
        public Platform GetById(Guid platform_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    dbPlatform result = (from n in db.dbPlatforms
                                     where (n.platform_id == platform_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        
        public void Invalidate(Guid platform_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbPlatforms
                        .Where(x => x.platform_id == platform_id)
                        .Update(x => new dbPlatform() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        public List<Platform> Find(int skip, int take, string keyword = "", string order_by = "", bool descending = false)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.dbPlatforms
                                where (keyword == "" 
                                    || p.platform_name.Contains(keyword)
                                )
                                select p);

                    List<dbPlatform> result = new List<dbPlatform>();

                    switch (order_by)
                    {
                        case "platform_name":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.platform_name).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.platform_name).Skip(skip).Take(take).ToList();
                            }
                            break;
                        
                        default:
                            result = data.OrderBy(s => s.platform_id).Skip(skip).Take(take).ToList();
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        


        
        
        public InterceptArgs<Platform> Intercept(Platform platform, bool forInsert)
        {
            InterceptArgs<Platform> args = new InterceptArgs<Platform>()
            {
                ForInsert = forInsert,
                ReturnEntity = platform
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void PerformIntercept(InterceptArgs<Platform> args);
        partial void PreProcess(Platform platform, bool forInsert);
        partial void AfterInsertPersisted(StencilContext db, dbPlatform platform);
        partial void BeforeUpdatePersisted(dbPlatform platform, Platform previous);
        partial void AfterUpdatePersisted(StencilContext db, dbPlatform platform, Platform previous);
        partial void AfterDeletePersisted(StencilContext db, dbPlatform platform);
        partial void AfterUpdateIndexed(StencilContext db, dbPlatform platform);
        partial void AfterInsertIndexed(StencilContext db, dbPlatform platform);
    }
}

