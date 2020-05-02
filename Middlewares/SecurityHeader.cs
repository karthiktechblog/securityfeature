using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KarthikTechBlog.SecurityFeatures.API
{
    /// <summary>
    /// A middleware for adding security headers.
    /// </summary>
    public class SecurityHeader
    {
        private readonly RequestDelegate _next;
        private readonly CspSettings _settings;
        private readonly SecurityHeaderService _securityHeaderService;

        private readonly ILogger _logger;

        public SecurityHeader(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            CspSettings settings)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _securityHeaderService = new SecurityHeaderService();
            _logger = loggerFactory.CreateLogger<SecurityHeader>();
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Task Invoke(HttpContext context)
        {
            if (_settings == null)
            {
                _logger.LogDebug("no csp settings found");
                return _next(context);
            }

            var isOptionsRequest = string.Equals(context.Request.Method, HttpMethods.Options, StringComparison.OrdinalIgnoreCase);
            if (!isOptionsRequest)
            {
                context.Response.OnStarting(OnResponseStarting, context);
                return _next(context);
            }

            return _next(context);
        }

        private Task OnResponseStarting(object state)
        {
            var context = (HttpContext)state;
            try
            {
                _securityHeaderService.ApplyResult(context.Response, _settings);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to set security header");
            }
            return Task.CompletedTask;
        }
    }
}
