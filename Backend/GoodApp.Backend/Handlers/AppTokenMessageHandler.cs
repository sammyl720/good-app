using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodApp.Backend.Handlers
{
    public class AppTokenMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method != HttpMethod.Options)
            {
                IEnumerable<string> appTokens;
                if (!request.Headers.TryGetValues("appToken", out appTokens) ||
                    string.IsNullOrEmpty(appTokens.FirstOrDefault()))
                {
                    var res = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    var tcs = new TaskCompletionSource<HttpResponseMessage>();
                    tcs.SetResult(res);
                    return tcs.Task;
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
