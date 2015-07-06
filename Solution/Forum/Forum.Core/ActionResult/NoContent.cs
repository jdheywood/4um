using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Forum.Core.ActionResult
{
    // 204
    public class NoContent : IHttpActionResult
    {
        private readonly HttpRequestMessage request;

        public NoContent(HttpRequestMessage request)
        {
            this.request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(request.CreateResponse(HttpStatusCode.NoContent));
        }
    }
}
