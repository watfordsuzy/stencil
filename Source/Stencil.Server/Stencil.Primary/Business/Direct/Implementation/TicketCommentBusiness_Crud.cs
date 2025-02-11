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
    public partial class TicketCommentBusiness : BusinessBase, ITicketCommentBusiness
    {
        public TicketCommentBusiness(IFoundation foundation)
            : base(foundation, "TicketComment")
        {
        }
        
        protected ITicketCommentSynchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve<ITicketCommentSynchronizer>();
            }
        }

        public TicketComment Insert(TicketComment insertTicketComment)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    

                    this.PreProcess(insertTicketComment, true);
                    var interception = this.Intercept(insertTicketComment, true);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertTicketComment.ticket_comment_id == Guid.Empty)
                    {
                        insertTicketComment.ticket_comment_id = Guid.NewGuid();
                    }
                    insertTicketComment.created_utc = DateTime.UtcNow;
                    insertTicketComment.updated_utc = insertTicketComment.created_utc;

                    dbTicketComment dbModel = insertTicketComment.ToDbModel();
                    
                    dbModel.InvalidateSync(this.DefaultAgent, "insert");

                    db.dbTicketComments.Add(dbModel);

                    db.SaveChanges();
                    
                    this.AfterInsertPersisted(db, dbModel);
                    
                    this.Synchronizer.SynchronizeItem(dbModel.ticket_comment_id, Availability.Retrievable);
                    this.AfterInsertIndexed(db, dbModel);
                    
                    this.DependencyCoordinator.TicketCommentInvalidated(Dependency.None, dbModel.ticket_comment_id);
                }
                return this.GetById(insertTicketComment.ticket_comment_id);
            });
        }
        public TicketComment Update(TicketComment updateTicketComment)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    this.PreProcess(updateTicketComment, false);
                    var interception = this.Intercept(updateTicketComment, false);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateTicketComment.updated_utc = DateTime.UtcNow;
                    
                    dbTicketComment found = (from n in db.dbTicketComments
                                    where n.ticket_comment_id == updateTicketComment.ticket_comment_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        TicketComment previous = found.ToDomainModel();
                        
                        found = updateTicketComment.ToDbModel(found);

                        this.BeforeUpdatePersisted(found, previous);

                        found.InvalidateSync(this.DefaultAgent, "updated");
                        db.SaveChanges();
                        
                        this.AfterUpdatePersisted(db, found, previous);
                        
                        this.Synchronizer.SynchronizeItem(found.ticket_comment_id, Availability.Retrievable);
                        this.AfterUpdateIndexed(db, found);
                        
                        this.DependencyCoordinator.TicketCommentInvalidated(Dependency.None, found.ticket_comment_id);
                    
                    }
                    
                    return this.GetById(updateTicketComment.ticket_comment_id);
                }
            });
        }
        public void Delete(Guid ticket_comment_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var db = base.CreateSQLContext())
                {
                    dbTicketComment found = (from a in db.dbTicketComments
                                    where a.ticket_comment_id == ticket_comment_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");
                        db.SaveChanges();
                        
                        this.AfterDeletePersisted(db, found);
                        
                        this.Synchronizer.SynchronizeItem(found.ticket_comment_id, Availability.Retrievable);
                        
                        this.DependencyCoordinator.TicketCommentInvalidated(Dependency.None, found.ticket_comment_id);
                    }
                }
            });
        }
        public void SynchronizationUpdate(Guid ticket_comment_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spTicketComment_SyncUpdate(ticket_comment_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spTicketComment_SyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        public void SynchronizationHydrateUpdate(Guid ticket_comment_id, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.spTicketComment_HydrateSyncUpdate(ticket_comment_id, success, sync_date_utc, sync_log);
                }
            });
        }
        public List<Guid?> SynchronizationHydrateGetInvalid(int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    return db.spTicketComment_HydrateSyncGetInvalid(retryPriorityThreshold, sync_agent).ToList();
                }
            });
        }
        
        public TicketComment GetById(Guid ticket_comment_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    dbTicketComment result = (from n in db.dbTicketComments
                                     where (n.ticket_comment_id == ticket_comment_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        public List<TicketComment> GetByTicketID(Guid ticket_id)
        {
            return base.ExecuteFunction("GetByTicketID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbTicketComments
                                     where (n.ticket_id == ticket_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        public List<TicketComment> GetByCommenterID(Guid account_id)
        {
            return base.ExecuteFunction("GetByCommenterID", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    var result = (from n in db.dbTicketComments
                                     where (n.commenter_id == account_id)
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        
        public void InvalidateForCommenterID(Guid account_id, string reason)
        {
            base.ExecuteMethod("InvalidateForCommenterID", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbTicketComments
                        .Where(x => x.commenter_id == account_id)
                        .Update(x => new dbTicketComment() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                    
                }
            });
        }
        
        public void Invalidate(Guid ticket_comment_id, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQLContext())
                {
                    db.dbTicketComments
                        .Where(x => x.ticket_comment_id == ticket_comment_id)
                        .Update(x => new dbTicketComment() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        
        


        
        
        public InterceptArgs<TicketComment> Intercept(TicketComment ticketcomment, bool forInsert)
        {
            InterceptArgs<TicketComment> args = new InterceptArgs<TicketComment>()
            {
                ForInsert = forInsert,
                ReturnEntity = ticketcomment
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void PerformIntercept(InterceptArgs<TicketComment> args);
        partial void PreProcess(TicketComment ticketcomment, bool forInsert);
        partial void AfterInsertPersisted(StencilContext db, dbTicketComment ticketcomment);
        partial void BeforeUpdatePersisted(dbTicketComment ticketcomment, TicketComment previous);
        partial void AfterUpdatePersisted(StencilContext db, dbTicketComment ticketcomment, TicketComment previous);
        partial void AfterDeletePersisted(StencilContext db, dbTicketComment ticketcomment);
        partial void AfterUpdateIndexed(StencilContext db, dbTicketComment ticketcomment);
        partial void AfterInsertIndexed(StencilContext db, dbTicketComment ticketcomment);
    }
}

