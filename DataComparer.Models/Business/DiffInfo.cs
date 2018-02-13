namespace DataComparer.Models.Business
{
    /// <summary>
    /// The information of a single difference between to data items.
    /// </summary>
    public class DiffInfo
    {
        /// <summary>
        /// Gets or sets the zero based offset of the difference.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the length of the difference.
        /// </summary>
        public int Length { get; set; } = 0; // Setting the default value explicitly.
    }
}
