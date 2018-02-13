using System.Numerics;

namespace DataComparer.Core.Helpers
{
    /// <summary>
    /// Helper methods for operations with numeric types.
    /// </summary>
    public static class NumericHelper
    {
        /// <summary>
        /// Creates the big int.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public static BigInteger CreateBigInt(byte[] bytes)
        {
            BigInteger bigInt = BigInteger.Zero;

            for (int i = 0; i < bytes.Length; i++)
            {
                bigInt <<= 8;
                bigInt |= bytes[i];
            }

            return bigInt;
        }
    }
}
