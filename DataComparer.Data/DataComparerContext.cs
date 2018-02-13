using DataComparer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataComparer.Data
{
    /// <summary>
    /// The default DB context.
    /// </summary>
    /// <seealso cref="DbContext" />
    public class DataComparerContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataComparerContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DataComparerContext(DbContextOptions<DataComparerContext> options)
            : base(options)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the difference.
        /// </summary>
        public DbSet<Difference> Difference { get; set; }

        /// <summary>
        /// Gets or sets the data item.
        /// </summary>
        public DbSet<DataItem> DataItem { get; set; }

        #endregion
    }
}
