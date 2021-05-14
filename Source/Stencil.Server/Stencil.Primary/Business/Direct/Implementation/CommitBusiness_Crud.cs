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
    public partial class CommitBusiness : BusinessBase, ICommitBusiness
    {
        public CommitBusiness(IFoundation foundation)
            : base(foundation, "Commit")
        {
        }
        
        

        public Commit Insert(Commit insertCommit)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    

                    this.PreProcess(insertCommit, true);
                    var interception = this.Intercept(insertCommit, true);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insertCommit.commit_id == Guid.Empty)
                    {
                        insertCommit.commit_id = Guid.NewGuid();
                    }
                    insertCommit.created_utc = DateTime.UtcNow;
                    insertCommit.updated_utc = insertCommit.created_utc;

                    dbCommit dbModel = insertCommit.ToDbModel();
                    
                    

                    db.dbCommits.Add(dbModel);

                    db.SaveChanges();
                    
                    this.AfterInsertPersisted(db, dbModel);
                    
                    
                    this.DependencyCoordinator.CommitInvalidated(Dependency.None, dbModel.commit_id);
                }
                return this.GetById(insertCommit.commit_id);
            });
        }
        public Commit Update(Commit updateCommit)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var db = base.CreateSQLContext())
                {
                    this.PreProcess(updateCommit, false);
                    var interception = this.Intercept(updateCommit, false);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    updateCommit.updated_utc = DateTime.UtcNow;
                    
                    dbCommit found = (from n in db.dbCommits
                                    where n.commit_id == updateCommit.commit_id
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        Commit previous = found.ToDomainModel();
                        
                        found = updateCommit.ToDbModel(found);

                        this.BeforeUpdatePersisted(found, previous);

                        
                        db.SaveChanges();
                        
                        this.AfterUpdatePersisted(db, found, previous);
                        
                        
                        this.DependencyCoordinator.CommitInvalidated(Dependency.None, found.commit_id);
                    
                    }
                    
                    return this.GetById(updateCommit.commit_id);
                }
            });
        }
        public void Delete(Guid commit_id)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var db = base.CreateSQLContext())
                {
                    dbCommit found = (from a in db.dbCommits
                                    where a.commit_id == commit_id
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        db.dbCommits.Remove(found);
                        
                        db.SaveChanges();
                        
                        this.AfterDeletePersisted(db, found);
                        
                        
                        this.DependencyCoordinator.CommitInvalidated(Dependency.None, found.commit_id);
                    }
                }
            });
        }
        
        public Commit GetById(Guid commit_id)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQLContext())
                {
                    dbCommit result = (from n in db.dbCommits
                                     where (n.commit_id == commit_id)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        
        


        
        
        public InterceptArgs<Commit> Intercept(Commit commit, bool forInsert)
        {
            InterceptArgs<Commit> args = new InterceptArgs<Commit>()
            {
                ForInsert = forInsert,
                ReturnEntity = commit
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void PerformIntercept(InterceptArgs<Commit> args);
        partial void PreProcess(Commit commit, bool forInsert);
        partial void AfterInsertPersisted(StencilContext db, dbCommit commit);
        partial void BeforeUpdatePersisted(dbCommit commit, Commit previous);
        partial void AfterUpdatePersisted(StencilContext db, dbCommit commit, Commit previous);
        partial void AfterDeletePersisted(StencilContext db, dbCommit commit);
        
    }
}

