namespace DataComparer.Models.Common
{
    /// <summary>
    /// Constants used application wide.
    /// </summary>
    public static class AppConstants
    {
        #region Dependency injection

        /// <summary>
        /// The DataComparer.Business assembly and namespace.
        /// </summary>
        public const string DataComparerBusiness = "DataComparer.Business";

        /// <summary>
        /// The DataComparer.Data assembly and namespace.
        /// </summary>
        public const string DataComparerData = "DataComparer.Data";

        /// <summary>
        /// The DataComparer.Business namespace.
        /// </summary>
        public const string DataComparerBusinessServices = "DataComparer.Business.Services";

        /// <summary>
        /// The DataComparer.Data namespace.
        /// </summary>
        public const string DataComparerDataRepositories = "DataComparer.Data.Repositories";

        /// <summary>
        /// The service classes suffix.
        /// </summary>
        public const string ServiceSuffix = "Service";

        /// <summary>
        /// The repository classes suffix.
        /// </summary>
        public const string RepositorySuffix = "Repository";

        #endregion

        #region Encoding

        /// <summary>
        /// RegEx that matches base64 strings.
        /// </summary>
        public const string Base64Regex = @"^[a-zA-Z0-9\+/]*={0,3}$";

        #endregion
    }
}
