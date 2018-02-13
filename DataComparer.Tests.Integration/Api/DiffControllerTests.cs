using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataComparer.Api;
using DataComparer.Api.Controllers;
using DataComparer.Data;
using DataComparer.Models.Business;
using DataComparer.Models.Enums;
using DataComparer.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace DataComparer.Tests.Integration.Api
{
    /// <summary>
    /// The <see cref="DiffController"/> integration test methods.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class DiffControllerTests : IDisposable
    {
        #region Fields

        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly DataComparerContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffControllerTests"/> class.
        /// </summary>
        public DiffControllerTests()
        {
            WebHostBuilder hostBuilder = new WebHostBuilder();
            hostBuilder.ConfigureServices(s => s.AddDbContext<DataComparerContext>(options =>
                options.UseSqlite("Data Source=comparer.integration.db")));

            _server = new TestServer(hostBuilder.UseStartup<Startup>());
            _client = _server.CreateClient();

            _dbContext = _server.Host.Services.GetRequiredService<DataComparerContext>();
            Task.Run(() => DataComparerInitializer.InitializeAsync(_dbContext));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Should save data item to the database.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task SaveDataItem_ShouldSaveData()
        {
            string data = "{ Data: \"4444\" }";
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/v1/diff/1/left", content);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.FullMatch"/> status.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task GetDiff_ShouldGetFullMatch()
        {
            await DataHelper.SeedAsync(_dbContext);
            HttpResponseMessage response = await _client.GetAsync("/v1/diff/1");
            DiffResult diff = await GetDiffFromResponseAsync(response);

            response.EnsureSuccessStatusCode();
            Assert.Equal(DiffResultType.FullMatch, diff.Type);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.SizeMatch"/> status.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task GetDiff_ShouldGetSizeMatch()
        {
            await DataHelper.SeedAsync(_dbContext);
            HttpResponseMessage response = await _client.GetAsync("/v1/diff/2");
            DiffResult diff = await GetDiffFromResponseAsync(response);

            response.EnsureSuccessStatusCode();
            Assert.Equal(DiffResultType.SizeMatch, diff.Type);
            Assert.Contains(diff.Diffs, d => d.Offset == 1 && d.Length == 2);
            Assert.Contains(diff.Diffs, d => d.Offset == 17 && d.Length == 3);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.SizeMismatch"/> status.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task GetDiff_ShouldGetSizeMismatch()
        {
            await DataHelper.SeedAsync(_dbContext);
            HttpResponseMessage response = await _client.GetAsync("/v1/diff/3");
            DiffResult diff = await GetDiffFromResponseAsync(response);

            response.EnsureSuccessStatusCode();
            Assert.Equal(DiffResultType.SizeMismatch, diff.Type);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.RightSideMissing"/> status.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task GetDiff_ShouldGetRightSideMissing()
        {
            await DataHelper.SeedAsync(_dbContext);
            HttpResponseMessage response = await _client.GetAsync("/v1/diff/4");
            DiffResult diff = await GetDiffFromResponseAsync(response);

            response.EnsureSuccessStatusCode();
            Assert.Equal(DiffResultType.RightSideMissing, diff.Type);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.LeftSideMissing"/> status.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task GetDiff_ShouldGetLeftSideMissing()
        {
            await DataHelper.SeedAsync(_dbContext);
            HttpResponseMessage response = await _client.GetAsync("/v1/diff/5");
            DiffResult diff = await GetDiffFromResponseAsync(response);

            response.EnsureSuccessStatusCode();
            Assert.Equal(DiffResultType.LeftSideMissing, diff.Type);
        }

        /// <summary>
        /// Should get <see cref="DiffResultType.EntryDoesNotExists"/> status.
        /// </summary>
        [Fact]
        [Trait("API.Integration", nameof(DiffController))]
        public async Task GetDiff_ShouldGetEntryDoesNotExists()
        {
            await DataHelper.SeedAsync(_dbContext);
            HttpResponseMessage response = await _client.GetAsync("/v1/diff/6");
            await GetDiffFromResponseAsync(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
                _dbContext.Database.EnsureDeletedAsync();
                _dbContext.Dispose();
                _server.Dispose();
                _client.Dispose();
            }
        }

        #endregion

        #region Private methods

        private async Task<DiffResult> GetDiffFromResponseAsync(HttpResponseMessage response) =>
            JsonConvert.DeserializeObject<DiffResult>(await response.Content.ReadAsStringAsync());

        #endregion
    }
}
