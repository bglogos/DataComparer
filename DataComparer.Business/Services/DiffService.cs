using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DataComparer.Core.Helpers;
using DataComparer.Core.Repositories;
using DataComparer.Core.Services;
using DataComparer.Models.Business;
using DataComparer.Models.Entities;
using DataComparer.Models.Enums;
using DataComparer.Models.Factories;

namespace DataComparer.Business.Services
{
    /// <summary>
    /// The default implementation of <see cref="IDiffService"/>.
    /// </summary>
    /// <seealso cref="IDiffService" />
    public class DiffService : IDiffService
    {
        #region Fields

        private readonly IDifferenceRepository _differenceRepository;
        private readonly IDataItemRepository _dataItemRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffService"/> class.
        /// </summary>
        public DiffService(
            IDifferenceRepository differenceRepository,
            IDataItemRepository dataItemRepository)
        {
            _differenceRepository = differenceRepository;
            _dataItemRepository = dataItemRepository;
        }

        #endregion

        #region Public methods

        /// See <see cref="IDiffService.SaveDataItemAsync(int, DiffItemSide, string)"/> for more information.
        public async Task<int> SaveDataItemAsync(int diffId, DiffItemSide itemSide, string encodedData)
        {
            // Assumption: We do not want to save invalid or empty data.
            if (!EncodingHelper.IsValidBase64Data(encodedData))
            {
                throw new ArgumentException("The input data is in incorrect format", nameof(encodedData));
            }

            DataItem item;
            bool doDiffExists = await _differenceRepository.AnyAsync(d => d.Id == diffId);

            if (!doDiffExists)
            {
                item = DataItemFactory.CreateDataItem(itemSide, encodedData);
                Difference diff = DifferenceFactory.CreateDifference(diffId, item);
                await _differenceRepository.CreateAsync(diff);
                await _differenceRepository.SaveChangesAsync();
            }
            else
            {
                // Assumption: If an item on the same side already exists, the code throws an error.
                bool doItemExists = await _dataItemRepository.AnyAsync(di => di.DifferenceId == diffId && di.Side == itemSide);

                if (doItemExists)
                {
                    throw new InvalidOperationException($"Diff with ID {diffId} already contains {itemSide.ToString().ToLower()} data item.");
                }

                item = DataItemFactory.CreateDataItem(diffId, itemSide, encodedData);
                await _dataItemRepository.CreateAsync(item);
                await _dataItemRepository.SaveChangesAsync();
            }

            return item.Id;
        }

        /// See <see cref="IDiffService.GetDiffResult(int)"/> for more information.
        public async Task<DiffResult> GetDiffResult(int diffId)
        {
            Difference diff = await _differenceRepository.GetFirstOrDefaultAsync(d => d.Id == diffId, "DataItems");
            return GetDiffResult(diff);
        }

        #endregion

        #region Private methods

        private DiffResult GetDiffResult(Difference diff)
        {
            DiffResult result = new DiffResult();

            if (diff == null)
            {
                result.Type = DiffResultType.EntryDoesNotExists;
            }
            else if (!diff.DataItems.Any(di => di.Side == DiffItemSide.Left))
            {
                result.Type = DiffResultType.LeftSideMissing;
            }
            else if (!diff.DataItems.Any(di => di.Side == DiffItemSide.Right))
            {
                result.Type = DiffResultType.RightSideMissing;
            }
            else
            {
                byte[] leftItem = Convert.FromBase64String(
                    diff.DataItems.First(di => di.Side == DiffItemSide.Left).Data);

                byte[] rightItem = Convert.FromBase64String(
                    diff.DataItems.First(di => di.Side == DiffItemSide.Right).Data);

                if (leftItem.SequenceEqual(rightItem))
                {
                    result.Type = DiffResultType.FullMatch;
                }
                else if (leftItem.Length == rightItem.Length)
                {
                    int binaryLength = leftItem.Length * 8; // Gets total count of bits.

                    result.Type = DiffResultType.SizeMatch;
                    result.Diffs = GetDiffInfoCollection(
                        NumericHelper.CreateBigInt(leftItem),
                        NumericHelper.CreateBigInt(rightItem),
                        binaryLength);
                }
                else
                {
                    result.Type = DiffResultType.SizeMismatch;
                }
            }

            return result;
        }

        private ICollection<DiffInfo> GetDiffInfoCollection(BigInteger leftBits, BigInteger rightBits, int maxBinaryLength)
        {
            ICollection<DiffInfo> diffInfoCollection = new List<DiffInfo>();
            BigInteger diffBits = leftBits ^ rightBits;

            while (!diffBits.IsZero)
            {
                DiffInfo diffInfo = GetSingleDiffInfo(diffBits, maxBinaryLength);
                diffInfoCollection.Add(diffInfo);

                diffBits >>= maxBinaryLength - diffInfo.Offset;
                maxBinaryLength = diffInfo.Offset;
            }

            return diffInfoCollection;
        }

        private DiffInfo GetSingleDiffInfo(BigInteger diffBits, int length)
        {
            // Trimming trailing zeroes.
            while ((diffBits & 1) == 0)
            {
                length--;
                diffBits >>= 1;
            }

            DiffInfo info = new DiffInfo();

            while ((diffBits & 1) == 1)
            {
                info.Length++;
                length--;
                diffBits >>= 1;
            }

            info.Offset = length;

            return info;
        }

        #endregion
    }
}
