using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;

namespace KarthikTechBlog.SecurityFeatures.API
{
    public class SecurityHeaderService
    {
        private readonly string[] RemoveHeader = new[] { "Server" };
        private readonly string CSP_HEADER = "Content-Security-Policy";

        private readonly IDictionary<string, string> SecurityHeader = new Dictionary<string, string>
        {
            // MIME type sniffing security protection
            { "X-Content-Type-Options", "nosniff" },
            // X-UA-Compatible - Ensure that IE and Chrome frame is using the latest rendering mode. Alternatively, use the HTML meta tag X-UA-Compatible "IE=edge"
            { "X-UA-Compatible", "IE=edge,chrome=1" },
            // download protection
            { "X-Download-Options", "noopen" },
            
             // The X-Frame-Options HTTP response header can be used to indicate whether or not a browser should
             // be allowed to render a page in a <frame>, <iframe> or <object> . Sites can use this to avoid clickjacking attacks,
             // by ensuring that their content is not embedded into other sites.
             // X-Frame-Options: DENY
             // X-Frame-Options: SAMEORIGIN
             // X-Frame-Options: ALLOW-FROM https://example.com/
            
            { "X-Frame-Options", "Deny" },
            { "X-XSS-Protection", "1; mode=block" },
            // The Referer header will be omitted entirely to avoid sending of url to oter domain. eg seding to CDN
            // there is chance that the referer url will have a token or password that you do not want to send to other domain/web server.
            { "Referrer-Policy", "no-referrer" }
        };

        internal void ApplyResult(HttpResponse response, CspSettings settings)
        {
            var headers = response.Headers;

            StringBuilder stb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(settings.Default))
                stb.Append($"default-src {settings.Default};");
            if (!string.IsNullOrWhiteSpace(settings.Image))
                stb.Append($"img-src {settings.Image};");
            if (!string.IsNullOrWhiteSpace(settings.Style))
                stb.Append($"style-src {settings.Style};");
            if (!string.IsNullOrWhiteSpace(settings.Font))
                stb.Append($"font-src {settings.Font};");
            if (!string.IsNullOrWhiteSpace(settings.Script))
                stb.Append($"script-src {settings.Script};");
            if (settings.BlockMixedContent)
                // block all mixed contents( force to use https )
                stb.Append("block-all-mixed-content;");
            if (settings.UseHttps)
                // redirect to https
                stb.Append("upgrade-insecure-requests;");

            headers[CSP_HEADER] = stb.ToString();
            foreach (var headerValuePair in SecurityHeader)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;
            }

            foreach (var header in RemoveHeader)
            {
                headers.Remove(header);
            }
        }
    }
}
