using System.Threading.Tasks;

namespace DataComparer.Data
{
    /// <summary>
    /// The default DB initializer.
    /// </summary>
    public static class DataComparerInitializer
    {
        /// <summary>
        /// Initializes the database asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        public static async Task InitializeAsync(DataComparerContext context)
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}
