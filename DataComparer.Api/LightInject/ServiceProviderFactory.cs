using System;
using System.Reflection;
using DataComparer.Models.Common;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DataComparer.Api.LightInject
{
    /// <summary>
    /// A factory class that create and fill with components a service provider.
    /// </summary>
    public static class ServiceProviderFactory
    {
        #region Fields

        private static readonly Func<string, Assembly> Load = assemblyName => Assembly.Load(new AssemblyName(assemblyName));

        #endregion

        #region Public methods

        /// <summary>
        /// Create LightInject service provider that resolves objects.
        /// </summary>
        /// <param name="serviceCollection">The current registered services.</param>
        public static IServiceProvider Create(IServiceCollection serviceCollection)
        {
            // Create DI container.
            // Property injection causes recursive dependency with ServiceType:Microsoft.AspNetCore.Razor.Language.RazorEngine.
            ServiceContainer container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            container.RegisterServices();
            IServiceProvider serviceProvider = container.CreateServiceProvider(serviceCollection);

            return serviceProvider;
        }

        #endregion

        #region Private methods

        private static void RegisterServices(this IServiceContainer container)
        {
            // Fixes issue when there is a continuation during asynchronous execution.
            container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();

            // Register common classes.
            container.Register<IHttpContextAccessor, HttpContextAccessor>(new PerContainerLifetime());

            // Register repositories.
            container.RegisterAssembly(
                Load(AppConstants.DataComparerData),
                () => new PerContextLifetime(container.GetInstance<IHttpContextAccessor>()),
                (serviceType, implementationType) =>
                    serviceType.GetTypeInfo().IsInterface &&
                    implementationType.Name.EndsWith(AppConstants.RepositorySuffix) &&
                    implementationType.Namespace.StartsWith(AppConstants.DataComparerDataRepositories));

            // Register services.
            container.RegisterAssembly(
                Load(AppConstants.DataComparerBusiness),
                () => new PerContextLifetime(container.GetInstance<IHttpContextAccessor>()),
                (serviceType, implementationType) =>
                    serviceType.GetTypeInfo().IsInterface &&
                    implementationType.Name.EndsWith(AppConstants.ServiceSuffix) &&
                    implementationType.Namespace.StartsWith(AppConstants.DataComparerBusinessServices));
        }

        #endregion
    }
}
