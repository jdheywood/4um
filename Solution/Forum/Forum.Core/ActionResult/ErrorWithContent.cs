using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Forum.Core.Models;

namespace Forum.Core.ActionResult
{
    public class ErrorWithContent : ResultWithContent<List<ErrorDto>>
    {
        public ErrorWithContent(HttpRequestMessage request, List<ErrorDto> content)
            : base(request, content)
        {
        }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.BadRequest; }
        }
    }
}