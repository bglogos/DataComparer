using System.Collections.Generic;
using DataComparer.Models.Entities;

namespace DataComparer.Models.Factories
{
    /// <summary>
    /// Factory methods for creating Difference types.
    /// </summary>
    public static class DifferenceFactory
    {
        /// <summary>
        /// Creates new <see cref="Difference" /> object.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dataItem">The data item.</param>
        public static Difference CreateDifference(int id, DataItem dataItem) =>
            new Difference
            {
                Id = id,
                DataItems = dataItem == null ?
                    new List<DataItem>() :
                    new List<DataItem> { dataItem }
            };
    }
}
