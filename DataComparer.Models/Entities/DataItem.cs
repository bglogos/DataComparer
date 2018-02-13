using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataComparer.Models.Enums;

namespace DataComparer.Models.Entities
{
    /// <summary>
    /// The data item entity.
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [Required]
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        public DiffItemSide Side { get; set; }

        /// <summary>
        /// Gets or sets the difference identifier.
        /// </summary>
        [ForeignKey(nameof(Difference))]
        public int DifferenceId { get; set; }
    }
}
