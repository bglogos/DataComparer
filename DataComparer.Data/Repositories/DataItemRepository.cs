using DataComparer.Core.Repositories;
using DataComparer.Data.Repositories.Abstract;
using DataComparer.Models.Entities;

namespace DataComparer.Data.Repositories
{
    /// <summary>
    /// The default implementation of <see cref="IDataItemRepository"/>.
    /// </summary>
    /// <seealso cref="Repository{DataItem}" />
    /// <seealso cref="IDataItemRepository" />
    public class DataItemRepository : Repository<DataItem>, IDataItemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataItemRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DataItemRepository(DataComparerContext context)
            : base(context)
        {
        }
    }
}
