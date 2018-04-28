using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UI.Services;

namespace UI.Data.Repositories
{
    public class GenericRepository<TEntity,TContext> : IGenericRepository<TEntity>
        where TEntity : class
         where TContext : DbContext
    {
        protected readonly TContext Context;
        protected IDialogService DialogService;

        protected GenericRepository(TContext context,
            IDialogService  dialogService)
        {
            DialogService = dialogService;
            Context = context;
        }

       
        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }
        public virtual TEntity GetById(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }
        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }
       
        
        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch 
            {
                DialogService.ShowInfoDialog("An error has occurred, Can't save data.");
            }
           
        }
    }
}
