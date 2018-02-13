using System;
using System.Linq;

namespace DataComparer.Core.Helpers
{
    /// <summary>
    /// Helper methods for operations with enums.
    /// </summary>
    public static class EnumHelper
    {
        #region Public methods

        /// <summary>
        /// Tries to parse string value to a given enum type.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        public static bool TryParse<TEnum>(string value, out TEnum result)
            where TEnum : struct
        {
            result = default(TEnum);

            if (IsDefined(typeof(TEnum), value) &&
                Enum.TryParse(value, true, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Private methods

        private static bool IsDefined(Type enumType, string value) =>
            Enum.GetNames(enumType)
                .Any(n => string.Compare(n, value, true) == 0);

        #endregion
    }
}
