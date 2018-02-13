using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataComparer.Business.Services;
using DataComparer.Core.Repositories;
using DataComparer.Models.Business;
using DataComparer.Models.Entities;
using DataComparer.Models.Enums;
using NSubstitute;
using Xunit;

namespace DataComparer.Tests.Unit.Services
{
    /// <summary>
    /// The <see cref="DiffService"/> test methods.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DiffServiceTests : IDisposable
    {
        #region Fields

        private readonly DiffService _service;
        private readonly IDifferenceRepository _differenceRepository;
        private readonly IDataItemRepository _dataItemRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffServiceTests"/> class.
        /// </summary>
        public DiffServiceTests()
        {
            _differenceRepository = Substitute.For<IDifferenceRepository>();
            _dataItemRepository = Substitute.For<IDataItemRepository>();
            _service = new DiffService(_differenceRepository, _dataItemRepository);
        }

        #endregion

        #region Service tests

        /// <summary>
        /// Should save new valid data to an existing diff entry.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task SaveDataItem_NewValidDataToExistingDiff_ShouldSave()
        {
            DiffItemSide itemSide = DiffItemSide.Left;
            _differenceRepository.AnyAsync(Arg.Any<Expression<Func<Difference, bool>>>()).ReturnsForAnyArgs(true);

            await _service.SaveDataItemAsync(TestConstants.DiffId, itemSide, TestConstants.EncryptedDataA);

            _dataItemRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<int>());
            await _dataItemRepository.Received().CreateAsync(Arg.Is<DataItem>(di => di.Data == TestConstants.EncryptedDataA));
            await _dataItemRepository.Received().SaveChangesAsync();
        }

        /// <summary>
        /// Should save new valid data to new diff entry.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task SaveDataItem_NewValidDataToNewDiff_ShouldSave()
        {
            DiffItemSide itemSide = DiffItemSide.Left;
            _differenceRepository.AnyAsync(d => d.Id == TestConstants.DiffId).ReturnsForAnyArgs(false);

            await _service.SaveDataItemAsync(TestConstants.DiffId, itemSide, TestConstants.EncryptedDataA);

            await _differenceRepository.Received().CreateAsync(Arg.Is<Difference>(d =>
                d.Id == TestConstants.DiffId &&
                d.DataItems.Any(di => di.Data == TestConstants.EncryptedDataA)));
            await _differenceRepository.Received().SaveChangesAsync();
        }

        /// <summary>
        /// Should overwrite old data item.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task SaveDataItem_DuplicatedData_ShouldThrowException()
        {
            DiffItemSide itemSide = DiffItemSide.Left;
            _differenceRepository.AnyAsync(d => d.Id == TestConstants.DiffId).ReturnsForAnyArgs(true);
            _dataItemRepository.AnyAsync(Arg.Any<Expression<Func<DataItem, bool>>>()).ReturnsForAnyArgs(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.SaveDataItemAsync(TestConstants.DiffId, itemSide, TestConstants.EncryptedDataA));

            await _differenceRepository.DidNotReceive().SaveChangesAsync();
        }

        /// <summary>
        /// Should throw <see cref="ArgumentException" /> when the encrypted data is invalid.
        /// </summary>
        /// <param name="data">The test data.</param>
        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task SaveDataItem_InvalidData_ShouldThrowException(string data)
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.SaveDataItemAsync(TestConstants.DiffId, DiffItemSide.Left, data));
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.EntryDoesNotExists"/> status.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task GetDiff_ShouldGetNonExisting()
        {
            DiffResult result = await _service.GetDiffResult(TestConstants.DiffId);
            Assert.True(result.Type == DiffResultType.EntryDoesNotExists);
            Assert.Null(result.Diffs);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.FullMatch"/> status.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task GetDiff_ShouldGetFullMatch()
        {
            _differenceRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Difference, bool>>>(), Arg.Any<string>())
                .ReturnsForAnyArgs(TestConstants.FullMatchDiff);

            DiffResult result = await _service.GetDiffResult(TestConstants.DiffId);
            Assert.True(result.Type == DiffResultType.FullMatch);
            Assert.Null(result.Diffs);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.SizeMismatch"/> status.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task GetDiff_ShouldGetSizeMismatch()
        {
            _differenceRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Difference, bool>>>(), Arg.Any<string>())
                .ReturnsForAnyArgs(TestConstants.SizeMismatchDiff);

            DiffResult result = await _service.GetDiffResult(TestConstants.DiffId);
            Assert.True(result.Type == DiffResultType.SizeMismatch);
            Assert.Null(result.Diffs);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.SizeMatch"/> status.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task GetDiff_ShouldGetSizeMatch()
        {
            _differenceRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Difference, bool>>>(), Arg.Any<string>())
                .ReturnsForAnyArgs(TestConstants.SizeMatchDiff);

            DiffResult result = await _service.GetDiffResult(TestConstants.DiffId);
            Assert.True(result.Type == DiffResultType.SizeMatch);
            Assert.Contains(result.Diffs, d => d.Offset == 17 && d.Length == 3);
            Assert.Contains(result.Diffs, d => d.Offset == 1 && d.Length == 2);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.LeftSideMissing"/> status.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task GetDiff_ShouldGetLeftSideMissing()
        {
            _differenceRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Difference, bool>>>(), Arg.Any<string>())
                .ReturnsForAnyArgs(TestConstants.RightOnlyDiff);

            DiffResult result = await _service.GetDiffResult(TestConstants.DiffId);
            Assert.True(result.Type == DiffResultType.LeftSideMissing);
            Assert.Null(result.Diffs);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.RightSideMissing"/> status.
        /// </summary>
        [Fact]
        [Trait("Services.Unit", nameof(DiffService))]
        public async Task GetDiff_ShouldGetRightSideMissing()
        {
            _differenceRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Difference, bool>>>(), Arg.Any<string>())
                .ReturnsForAnyArgs(TestConstants.LeftOnlyDiff);

            DiffResult result = await _service.GetDiffResult(TestConstants.DiffId);
            Assert.True(result.Type == DiffResultType.RightSideMissing);
            Assert.Null(result.Diffs);
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _differenceRepository.ClearReceivedCalls();
                _dataItemRepository.ClearReceivedCalls();
            }
        }

        #endregion
    }
}
