using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Xml.Linq;

namespace Client.Extensions
{
    public class CookieHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            request.Headers.Add("Prefer", "odata.maxpagesize = 1000");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}