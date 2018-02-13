using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataComparer.Core.Repositories
{
    /// <summary>
    /// The generic reposotory.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets the total entities count.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets all entities from database.
        /// </summary>
        IQueryable<TEntity> All();

        /// <summary>
        /// Gets an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key.</param>
        /// <returns>The entity with the required ID.</returns>
        TEntity GetById(object id);

        /// <summary>
        /// Gets entities via optional filter and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties);

        /// <summary>
        /// Gets entities via optional filter, sort order, and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);

        /// <summary>
        /// Gets the first or default entity via optional filter and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, string includeProperties);

        /// <summary>
        /// Gets the first or default entity via optional filter, sort order, and includes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);

        /// <summary>
        /// Gets entities from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter.</param>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets entities from database with filting and paging.
        /// </summary>
        /// <param name="filter">Specified a filter.</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = int.MaxValue);

        /// <summary>
        /// Gets the entity(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate"> Specified the filter expression.</param>
        bool Contains(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find entity by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        TEntity Find(params object[] keys);

        /// <summary>
        /// Find entity by specified expression.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The required entity.</returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Create a new entity to database.
        /// </summary>
        /// <param name="entity">Specified a new entity to create.</param>
        void Create(TEntity entity);

        /// <summary>
        /// Deletes the entity by primary key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(object id);

        /// <summary>
        /// Delete the entity from database.
        /// </summary>
        /// <param name="entity">Specified a existing entity to delete.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities from database by specified filter expression.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Update entity changes and save to database.
        /// </summary>
        /// <param name="entity">Specified the entity to save.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Attaches the given entity to the context and set the properties that are modified and need to be updated in db.
        /// </summary>
        /// <param name="entity">The entity to be attacted to the context.</param>
        /// <param name="changedProperties">The properties that are changed, these are the only properties that will be updated in db.</param>
        void Attach(TEntity entity, params string[] changedProperties);

        /// <summary>
        /// Counts asynchronous.
        /// </summary>
        /// <returns>The count of the entries.</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Gets entry by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The entry.</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets entities via optional filter and includes asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties);

        /// <summary>
        /// Gets entities via optional filter, sort order, and includes asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns><see cref="IQueryable{TEntity}"/> filtered object.</returns>
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);

        /// <summary>
        /// Determines whether the entity(s) exists in database by specified filter asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> if the data set contains the predicate.</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds entry asynchronous.
        /// </summary>
        /// <param name="keys">The primary keys.</param>
        /// <returns>The entry.</returns>
        Task<TEntity> FindAsync(params object[] keys);

        /// <summary>
        /// Finds entry by predicate asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The entry.</returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Create a new entity to database asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        Task<int> SaveChangesAsync();
    }
}
