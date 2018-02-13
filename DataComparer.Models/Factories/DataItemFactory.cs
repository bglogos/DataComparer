using DataComparer.Models.Entities;
using DataComparer.Models.Enums;

namespace DataComparer.Models.Factories
{
    /// <summary>
    /// Factory methods for creating DataItem types.
    /// </summary>
    public static class DataItemFactory
    {
        /// <summary>
        /// Creates new <see cref="DataItem"/> object.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <param name="encodedData">The encoded data.</param>
        public static DataItem CreateDataItem(DiffItemSide side, string encodedData) =>
            CreateDataItem(default(int), side, encodedData);

        /// <summary>
        /// Creates new <see cref="DataItem"/> object.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <param name="side">The side.</param>
        /// <param name="encodedData">The encoded data.</param>
        public static DataItem CreateDataItem(int diffId, DiffItemSide side, string encodedData) =>
            new DataItem
            {
                DifferenceId = diffId,
                Side = side,
                Data = encodedData
            };
    }
}
