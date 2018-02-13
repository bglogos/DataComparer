using System.Collections.Generic;
using DataComparer.Models.Business;
using DataComparer.Models.Entities;
using DataComparer.Models.Enums;

namespace DataComparer.Tests.Unit
{
    /// <summary>
    /// Constants used in the unit tests.
    /// </summary>
    public static class TestConstants
    {
        /// <summary>
        /// Default difference identifier.
        /// </summary>
        public const int DiffId = 1;

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
        /// Data item request created from <see cref="EncryptedDataA"/>.
        /// </summary>
        public static readonly DataItemRequest DataItemRequestA = new DataItemRequest { Data = EncryptedDataA };

        /// <summary>
        /// Left data item with <see cref="EncryptedDataA"/>.
        /// </summary>
        public static readonly DataItem DataItemA = new DataItem
        {
            Id = 1,
            Side = DiffItemSide.Left,
            Data = EncryptedDataA,
            DifferenceId = DiffId
        };

        /// <summary>
        /// Right data item with <see cref="EncryptedDataB"/>.
        /// </summary>
        public static readonly DataItem DataItemB = new DataItem
        {
            Id = 2,
            Side = DiffItemSide.Right,
            Data = EncryptedDataB,
            DifferenceId = DiffId
        };

        /// <summary>
        /// Right data item with <see cref="EncryptedDataB"/>.
        /// </summary>
        public static readonly DataItem DataItemC = new DataItem
        {
            Id = 3,
            Side = DiffItemSide.Right,
            Data = EncryptedDataC,
            DifferenceId = DiffId
        };

        /// <summary>
        /// Right data item with <see cref="EncryptedDataA"/>.
        /// </summary>
        public static readonly DataItem DataItemD = new DataItem
        {
            Id = 4,
            Side = DiffItemSide.Right,
            Data = EncryptedDataA,
            DifferenceId = DiffId
        };

        /// <summary>
        /// Difference entry containing same data items.
        /// </summary>
        public static readonly Difference FullMatchDiff = new Difference
        {
            Id = DiffId,
            DataItems = new List<DataItem> { DataItemA, DataItemD }
        };

        /// <summary>
        /// Difference entry containing same size data items.
        /// </summary>
        public static readonly Difference SizeMatchDiff = new Difference
        {
            Id = DiffId,
            DataItems = new List<DataItem> { DataItemA, DataItemB }
        };

        /// <summary>
        /// Difference entry containing different size data items.
        /// </summary>
        public static readonly Difference SizeMismatchDiff = new Difference
        {
            Id = DiffId,
            DataItems = new List<DataItem> { DataItemA, DataItemC }
        };

        /// <summary>
        /// Difference entry containing left item only.
        /// </summary>
        public static readonly Difference LeftOnlyDiff = new Difference
        {
            Id = DiffId,
            DataItems = new List<DataItem> { DataItemA }
        };

        /// <summary>
        /// Difference entry containing right item only.
        /// </summary>
        public static readonly Difference RightOnlyDiff = new Difference
        {
            Id = DiffId,
            DataItems = new List<DataItem> { DataItemB }
        };
    }
}
