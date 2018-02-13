using System;
using System.Threading.Tasks;
using DataComparer.Api.Controllers;
using DataComparer.Core.Services;
using DataComparer.Models.Business;
using DataComparer.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace DataComparer.Tests.Unit.Api
{
    /// <summary>
    /// The <see cref="DiffController"/> unit test methods.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class DiffControllerTests : IDisposable
    {
        #region Fields

        private readonly DiffController _controller;
        private readonly IDiffService _diffService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffControllerTests"/> class.
        /// </summary>
        public DiffControllerTests()
        {
            _diffService = Substitute.For<IDiffService>();
            _controller = new DiffController(_diffService);
        }

        #endregion

        #region Controller tests

        /// <summary>
        /// Should save left data item and return OK result.
        /// </summary>
        [Fact]
        [Trait("API.Unit", nameof(DiffController))]
        public async Task SaveDataItem_LeftData_ShouldSave()
        {
            IActionResult result = await _controller.SaveDataItemAsync(TestConstants.DiffId, "left", TestConstants.DataItemRequestA);

            await _diffService.Received().SaveDataItemAsync(TestConstants.DiffId, DiffItemSide.Left, TestConstants.DataItemRequestA.Data);
            Assert.IsType<CreatedResult>(result);
        }

        /// <summary>
        /// Should save right data item and return OK result.
        /// </summary>
        [Fact]
        [Trait("API.Unit", nameof(DiffController))]
        public async Task SaveDataItem_RightData_ShouldSave()
        {
            IActionResult result = await _controller.SaveDataItemAsync(TestConstants.DiffId, "right", TestConstants.DataItemRequestA);

            await _diffService.Received().SaveDataItemAsync(TestConstants.DiffId, DiffItemSide.Right, TestConstants.DataItemRequestA.Data);
            Assert.IsType<CreatedResult>(result);
        }

        /// <summary>
        /// Should not allow passing the side as numeric value and return not found result.
        /// </summary>
        [Fact]
        [Trait("API.Unit", nameof(DiffController))]
        public async Task SaveDataItem_NumericDataSide_ShouldNotFind()
        {
            IActionResult result = await _controller.SaveDataItemAsync(TestConstants.DiffId, "1", TestConstants.DataItemRequestA);

            await _diffService.DidNotReceiveWithAnyArgs().SaveDataItemAsync(Arg.Any<int>(), Arg.Any<DiffItemSide>(), Arg.Any<string>());
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Should compare existing entry.
        /// </summary>
        [Fact]
        [Trait("API.Unit", nameof(DiffController))]
        public async Task GetComparison_ExistingEntry_ShouldCompare()
        {
            _diffService.GetDiffResult(TestConstants.DiffId).Returns(new DiffResult { Type = DiffResultType.SizeMismatch });

            IActionResult result = await _controller.GetComparisonResultAsync(TestConstants.DiffId);

            await _diffService.Received().GetDiffResult(TestConstants.DiffId);
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        /// Should not compare non-existing entry and return not found result.
        /// </summary>
        [Fact]
        [Trait("API.Unit", nameof(DiffController))]
        public async Task GetComparison_NonExistingEntry_ShouldNotFind()
        {
            _diffService.GetDiffResult(TestConstants.DiffId).Returns(new DiffResult { Type = DiffResultType.EntryDoesNotExists });

            IActionResult result = await _controller.GetComparisonResultAsync(TestConstants.DiffId);

            await _diffService.Received().GetDiffResult(TestConstants.DiffId);
            Assert.IsType<NotFoundResult>(result);
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
                _diffService.ClearReceivedCalls();
                _controller.Dispose();
            }
        }

        #endregion
    }
}
