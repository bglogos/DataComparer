namespace DataComparer.Models.Enums
{
    /// <summary>
    /// The result type of a comparison operation.
    /// </summary>
    public enum DiffResultType
    {
        /// <summary>
        /// The diff entry does not exist in the storage.
        /// </summary>
        EntryDoesNotExists,

        /// <summary>
        /// There is no left side entry.
        /// </summary>
        LeftSideMissing,

        /// <summary>
        /// There is no right side entry.
        /// </summary>
        RightSideMissing,

        /// <summary>
        /// Both data items matches completely.
        /// </summary>
        FullMatch,

        /// <summary>
        /// The data items have the same size but different content.
        /// </summary>
        SizeMatch,

        /// <summary>
        /// The data items have different sizes.
        /// </summary>
        SizeMismatch
    }
}
