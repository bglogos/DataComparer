using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataComparer.Tests.Integration.Helpers
{
    /// <summary>
    /// Helper methods for setting up test data.
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Seeds the specified context asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        public static async Task SeedAsync(DbContext context)
        {
            await context.AddRangeAsync(
                TestConstants.FullMatchDiff,
                TestConstants.SizeMatchDiff,
                TestConstants.SizeMismatchDiff,
                TestConstants.LeftOnlyDiff,
                TestConstants.RightOnlyDiff);
            await context.SaveChangesAsync();
        }
    }
}
