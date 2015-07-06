using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Forum.Core.ActionResult
{
    public abstract class ResultWithContent<T> : IHttpActionResult
    {
        private readonly HttpRequestMessage request;

        private readonly T content;

        protected ResultWithContent(HttpRequestMessage request, T content)
        {
            this.request = request;
            this.content = content;
        }

        protected abstract HttpStatusCode StatusCode { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var httpResponseMessage = request.CreateResponse(StatusCode, content);

            return Task.FromResult(httpResponseMessage);
        }
    }
}