using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataComparer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataComparer.Data.Repositories.Abstract
{
    /// <summary>
    /// The default implementation of <see cref="IRepository{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        #region  Fields

        /// <summary>
        /// Current database context.
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Current entity set.
        /// </summary>
        private readonly DbSet<TEntity> _dbSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        #endregion

        #region IRepository public methods

        /// <summary>
        /// Gets the total entities count.
        /// </summary>
        public virtual int Count => _dbSet.Count();

        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        public virtual IQueryable<TEntity> All() => _dbSet.AsQueryable();

        /// <summary>
        /// Gets an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>The entity with the required ID.</returns>
        public virtual TEntity GetById(object id) => _dbSet.Find(id);

        /// <summary>
        /// Gets entities via optional filter, sort order, and includes.
        /// </summary>
        /// <param name="filter">A function to test each element for a condition.</param>
        /// <param name="includeProperties">The dot-separated list of related objects to return in the query results and then comma-separated to accept list of properties.</param>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties) =>
            Get(filter, null, includeProperties);

        /// <summary>
        /// Gets entities via optional filter, sort order, and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return orderBy == null ?
                query :
                orderBy(query);
        }

        /// <summary>
        /// Gets entities from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter.</param>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate) =>
            _dbSet.Where(predicate).AsQueryable();

        /// <summary>
        /// Gets entities from database with filting and paging.
        /// </summary>
        /// <param name="filter">Specified a filter.</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = int.MaxValue)
        {
            int skipCount = index * size;
            IQueryable<TEntity> resetSet = filter != null ? _dbSet.Where(filter).AsQueryable() : _dbSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Gets the entity(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate"> Specified the filter expression.</param>
        public bool Contains(Expression<Func<TEntity, bool>> predicate) => _dbSet.Count(predicate) > 0;

        /// <summary>
        /// Find entity by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        public virtual TEntity Find(params object[] keys) => _dbSet.Find(keys);

        /// <summary>
        /// Find entity by specified expression.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The required entity.</returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate) => _dbSet.FirstOrDefault(predicate);

        /// <summary>
        /// Create a new entity to database.
        /// </summary>
        /// <param name="entity">Specified a new entity to create.</param>
        public virtual void Create(TEntity entity) => _dbSet.Add(entity);

        /// <summary>
        /// Deletes the entity by primary key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);

            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        /// <summary>
        /// Delete the entity from database.
        /// </summary>
        /// <param name="entity">Specified a existing entity to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete entities from database by specified filter expression.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = Filter(predicate);
            foreach (var entity in entitiesToDelete)
            {
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }

                _dbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Update entity changes and save to database.
        /// </summary>
        /// <param name="entity">Specified the entity to save.</param>
        public virtual void Update(TEntity entity) => _dbContext.Entry(entity).State = EntityState.Modified;

        /// <summary>
        /// Attaches the given entity to the context and set the properties that are modified and need to be updated in db.
        /// </summary>
        /// <param name="entity">The entity to be attacted to the context.</param>
        /// <param name="changedProperties">The properties that are changed, these are the only properties that will be updated in db.</param>
        public virtual void Attach(TEntity entity, params string[] changedProperties)
        {
            _dbSet.Attach(entity);
            foreach (string property in changedProperties)
            {
                _dbContext.Entry(entity).Property(changedProperties[0]).IsModified = true;
            }
        }

        /// <summary>
        /// Counts asynchronous.
        /// </summary>
        /// <returns>The count of the entries.</returns>
        public virtual Task<int> CountAsync() => _dbSet.CountAsync();

        /// <summary>
        /// Gets entry by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The entry.</returns>
        public virtual Task<TEntity> GetByIdAsync(int id) => _dbSet.FindAsync(id);

        /// <summary>
        /// Gets entities via optional filter and includes asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <see cref="T:System.Linq.IQueryable`1" /> filtered object.
        /// </returns>
        public virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties) =>
            GetAsync(filter, null, includeProperties);

        /// <summary>
        /// Gets entities via optional filter, sort order, and includes asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <see cref="T:System.Linq.IQueryable`1" /> filtered object.
        /// </returns>
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null) =>
            await Get(filter, orderBy, includeProperties).ToListAsync();

        /// <summary>
        /// Gets the first or default entity via optional filter and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <see cref="T:System.Linq.IQueryable`1" /> filtered object.
        /// </returns>
        public virtual Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, string includeProperties) =>
            GetFirstOrDefaultAsync(filter, null, includeProperties);

        /// <summary>
        /// Gets the first or default entity via optional filter, sort order, and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        ///   <see cref="T:System.Linq.IQueryable`1" /> filtered object.
        /// </returns>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null) =>
            await Get(filter, orderBy, includeProperties).FirstOrDefaultAsync();

        /// <summary>
        /// Determines whether the entity(s) exists in database by specified filter asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> if the data set contains the predicate.</returns>
        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => _dbSet.AnyAsync(predicate);

        /// <summary>
        /// Finds entry asynchronous.
        /// </summary>
        /// <param name="keys">The primary keys.</param>
        /// <returns>The entry.</returns>
        public virtual Task<TEntity> FindAsync(params object[] keys) => _dbSet.FindAsync(keys);

        /// <summary>
        /// Finds entry by predicate asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The entry.</returns>
        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate) => _dbSet.FirstOrDefaultAsync(predicate);

        /// <summary>
        /// Create a new entity to database asynchronous.
        /// </summary>
        /// <param name="entity">Specified a new entity to create.</param>
        public virtual Task CreateAsync(TEntity entity) => _dbSet.AddAsync(entity);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        public virtual int SaveChanges() => _dbContext.SaveChanges();

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        public virtual Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();

        #endregion

        #region IDisposable public methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        #endregion
    }
}
