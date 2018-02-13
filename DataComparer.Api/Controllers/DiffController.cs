using System;
using System.Threading.Tasks;
using DataComparer.Core.Helpers;
using DataComparer.Core.Services;
using DataComparer.Models.Business;
using DataComparer.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DataComparer.Api.Controllers
{
    /// <summary>
    /// The Diff web API controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Produces("application/json")]
    [ApiVersion("1")]
    [Route("~/v{version:apiVersion}/[controller]")]
    public class DiffController : Controller
    {
        #region Fields

        private readonly IDiffService _diffService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffController"/> class.
        /// </summary>
        public DiffController(IDiffService diffService)
        {
            _diffService = diffService;
        }

        #endregion

        #region Action methods

        /// <summary>
        /// Saves base64 encoded data item asynchronous.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        /// <param name="itemSideName">The item side name.</param>
        /// <param name="request">The request.</param>
        [HttpPost("{diffId}/{itemSideName}")]
        public async Task<IActionResult> SaveDataItemAsync([FromRoute] int diffId, [FromRoute] string itemSideName, [FromBody] DataItemRequest request)
        {
            if (!EnumHelper.TryParse(itemSideName, out DiffItemSide itemSide))
            {
                return NotFound();
            }

            try
            {
                await _diffService.SaveDataItemAsync(diffId, itemSide, request?.Data);
                return Created($"/v1/diff/{diffId}", diffId);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the comparison result asynchronous.
        /// </summary>
        /// <param name="diffId">The difference identifier.</param>
        [HttpGet("{diffId}")]
        public async Task<IActionResult> GetComparisonResultAsync([FromRoute] int diffId)
        {
            DiffResult result = await _diffService.GetDiffResult(diffId);

            // Assumption: Returning information whether one of the compared data items is missing.
            return result.Type == DiffResultType.EntryDoesNotExists ?
                (IActionResult)NotFound() :
                Ok(result);
        }

        #endregion
    }
}