using System;
using LightInject;
using Microsoft.AspNetCore.Http;

namespace DataComparer.Api.LightInject
{
    /// <summary>
    /// Lifetime per HTTP context.
    /// </summary>
    public class PerContextLifetime : ILifetime
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PerContextLifetime"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public PerContextLifetime(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a service instance according to the specific lifetime characteristics.
        /// </summary>
        /// <param name="createInstance">The function delegate used to create a new service instance.</param>
        /// <param name="scope">The <see cref="T:LightInject.Scope" /> of the current service request.</param>
        /// <returns>
        /// The requested services instance.
        /// </returns>
        public object GetInstance(Func<object> createInstance, Scope scope)
        {
            bool hasContext = _httpContextAccessor?.HttpContext != null;

            if (hasContext)
            {
                PerScopeLifetime perScopeLifetime = new PerScopeLifetime();
                return perScopeLifetime.GetInstance(createInstance, scope);
            }

            return createInstance();
        }

        #endregion
    }
}
