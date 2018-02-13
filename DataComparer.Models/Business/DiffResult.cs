using System.Collections.Generic;
using DataComparer.Models.Enums;

namespace DataComparer.Models.Business
{
    /// <summary>
    /// The result of a comparison.
    /// </summary>
    public class DiffResult
    {
        /// <summary>
        /// Gets or sets the result type.
        /// </summary>
        public DiffResultType Type { get; set; }

        /// <summary>
        /// Gets or sets the diffs information.
        /// </summary>
        public ICollection<DiffInfo> Diffs { get; set; }
    }
}
