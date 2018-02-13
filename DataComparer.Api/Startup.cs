using System;
using DataComparer.Api.LightInject;
using DataComparer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataComparer.Api
{
    /// <summary>
    /// Web application startup class.
    /// </summary>
    public class Startup
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add MVC and API controllers resolvers.
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            // Add data protection. Isolate the application from other applications.
            services.AddDataProtection();

            // Assumption: Having the maximum request body size of 10 MB.
            services.Configure<FormOptions>(option =>
            {
                option.MultipartBodyLengthLimit = 10485760; // 1024 * 1024 * 10; 10 MB
            });

            // Reporting API versions will return the headers "api-supported-versions" and "api-deprecated-versions".
            services.AddApiVersioning(sa =>
            {
                sa.ReportApiVersions = true;
                sa.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Add DB connection.
            services.AddDbContext<DataComparerContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("ComparerConnection")));

            // DI framework factory.
            return ServiceProviderFactory.Create(services);
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        #endregion
    }
}
