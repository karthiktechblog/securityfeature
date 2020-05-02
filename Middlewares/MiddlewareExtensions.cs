using Microsoft.AspNetCore.Builder;
using System;

namespace KarthikTechBlog.SecurityFeatures.API
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeader(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<SecurityHeader>(new CspSettings
            {
                Default = "'self' https://*.karthiktechblog.com",
                Image = "'self' data: https://*.karthiktechblog.com",
                Style = "'self' 'unsafe-inline' https://*.karthiktechblog.com",
                Font = "'self' data: * https://*.karthiktechblog.com",
                Script = "'self' 'unsafe-inline' https://*.karthiktechblog.com",
                BlockMixedContent = true,
                UseHttps = true
            });
        }

        public static IApplicationBuilder UseSecurityHeader(
            this IApplicationBuilder app,
            Action<CspSettings> configure)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var settings = new CspSettings();
            configure(settings);
            return app.UseMiddleware<SecurityHeader>(settings);
        }
    }
}
