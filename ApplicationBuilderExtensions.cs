using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace KarthikTechBlog.SecurityFeatures.API
{
    /// <summary>
    /// A simple middleware to process the request to use UseSecurityHeaders
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Security Header middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app,
            IConfiguration configuration,
            IWebHostEnvironment env)
            => MiddlewareExtensions.UseSecurityHeader(app, (settings) =>
            {
                settings.Default = configuration["SecurityHeader:CSP:default"];
                settings.Image = configuration["SecurityHeader:CSP:image"];
                settings.Style = configuration["SecurityHeader:CSP:style"];
                settings.Font = configuration["SecurityHeader:CSP:font"];
                settings.Script = configuration["SecurityHeader:CSP:script"];
                settings.BlockMixedContent = !env.IsDevelopment();
                settings.UseHttps = !env.IsDevelopment();
            });
    }
}
