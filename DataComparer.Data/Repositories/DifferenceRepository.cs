using DataComparer.Core.Repositories;
using DataComparer.Data.Repositories.Abstract;
using DataComparer.Models.Entities;

namespace DataComparer.Data.Repositories
{
    /// <summary>
    /// The default implementation of <see cref="IDifferenceRepository"/>.
    /// </summary>
    /// <seealso cref="Repository{Difference}" />
    /// <seealso cref="IDifferenceRepository" />
    public class DifferenceRepository : Repository<Difference>, IDifferenceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DifferenceRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DifferenceRepository(DataComparerContext context)
            : base(context)
        {
        }
    }
}
