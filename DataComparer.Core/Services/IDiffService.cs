using System;
using System.Threading.Tasks;
using DataComparer.Models.Business;
using DataComparer.Models.Entities;
using DataComparer.Models.Enums;

namespace DataComparer.Core.Services
{
    /// <summary>
    /// The service for getting, setting and comparing data.
    /// </summary>
    public interface IDiffService
    {
        /// <summary>
        /// Saves the binary data asynchronous.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <param name="itemSide">The item side.</param>
        /// <param name="encodedData">The encoded data.</param>
        /// <returns>The ID of the newly created <see cref="DataItem"/>.</returns>
        /// <exception cref="ArgumentException">The input data is in incorrect format - encodedData</exception>
        /// <exception cref="InvalidOperationException">Diff with the given ID already contains an item on the selected side.</exception>
        Task<int> SaveDataItemAsync(int diffId, DiffItemSide itemSide, string encodedData);

        /// <summary>
        /// Gets the result of a comparison.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <returns>The result of the comparison.</returns>
        Task<DiffResult> GetDiffResult(int diffId);
    }
}
