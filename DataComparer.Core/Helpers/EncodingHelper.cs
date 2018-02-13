using System.Text.RegularExpressions;
using DataComparer.Models.Common;

namespace DataComparer.Core.Helpers
{
    /// <summary>
    /// Helper methods for operations with encoded data.
    /// </summary>
    public static class EncodingHelper
    {
        /// <summary>
        /// Determines whether the given string is valid base64 data.
        /// RegEx source: https://stackoverflow.com/questions/6309379/how-to-check-for-a-valid-base64-encoded-string
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if the given string is valid base64 data; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidBase64Data(string data)
        {
            string trimmedData = data.Trim();
            return !string.IsNullOrWhiteSpace(data) &&
                (trimmedData.Length % 4 == 0) &&
                Regex.IsMatch(trimmedData, AppConstants.Base64Regex, RegexOptions.None);
        }
    }
}
