using System.Collections.Generic;
using DataComparer.Models.Entities;
using DataComparer.Models.Enums;

namespace DataComparer.Tests.Integration
{
    /// <summary>
    /// Constants used in the unit tests.
    /// </summary>
    public static class TestConstants
    {
        /// <summary>
        /// Type A encrypted data.
        /// Binary: 11100011 10001110 00111000
        /// </summary>
        public const string EncryptedDataA = "4444";

        /// <summary>
        /// Type B encrypted data.
        /// Binary: 10000011 10001110 01001000
        /// </summary>
        public const string EncryptedDataB = "g45I";

        /// <summary>
        /// Type C encrypted data.
        /// Binary: 11100011 10001110 00111000 00010010
        /// </summary>
        public const string EncryptedDataC = "4444Eg==";

        /// <summary>
        /// Difference entry containing same data items.
        /// </summary>
        public static readonly Difference FullMatchDiff = new Difference
        {
            Id = 1,
            DataItems = new List<DataItem>
            {
                new DataItem
                {
                    Data = EncryptedDataA,
                    Side = DiffItemSide.Left
                },
                new DataItem
                {
                    Data = EncryptedDataA,
                    Side = DiffItemSide.Right
                },
            }
        };

        /// <summary>
        /// Difference entry containing same size data items.
        /// </summary>
        public static readonly Difference SizeMatchDiff = new Difference
        {
            Id = 2,
            DataItems = new List<DataItem>
            {
                new DataItem
                {
                    Data = EncryptedDataA,
                    Side = DiffItemSide.Left
                },
                new DataItem
                {
                    Data = EncryptedDataB,
                    Side = DiffItemSide.Right
                },
            }
        };

        /// <summary>
        /// Difference entry containing different size data items.
        /// </summary>
        public static readonly Difference SizeMismatchDiff = new Difference
        {
            Id = 3,
            DataItems = new List<DataItem>
            {
                new DataItem
                {
                    Data = EncryptedDataA,
                    Side = DiffItemSide.Left
                },
                new DataItem
                {
                    Data = EncryptedDataC,
                    Side = DiffItemSide.Right
                },
            }
        };

        /// <summary>
        /// Difference entry containing left item only.
        /// </summary>
        public static readonly Difference LeftOnlyDiff = new Difference
        {
            Id = 4,
            DataItems = new List<DataItem>
            {
                new DataItem
                {
                    Data = EncryptedDataA,
                    Side = DiffItemSide.Left
                }
            }
        };

        /// <summary>
        /// Difference entry containing right item only.
        /// </summary>
        public static readonly Difference RightOnlyDiff = new Difference
        {
            Id = 5,
            DataItems = new List<DataItem>
            {
                new DataItem
                {
                    Data = EncryptedDataA,
                    Side = DiffItemSide.Right
                },
            }
        };
    }
}
