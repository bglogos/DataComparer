using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataComparer.Models.Entities
{
    /// <summary>
    /// The difference entity.
    /// </summary>
    public class Difference
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data items.
        /// </summary>
        public ICollection<DataItem> DataItems { get; set; }
    }
}
