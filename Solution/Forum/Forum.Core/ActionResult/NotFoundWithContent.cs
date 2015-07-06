using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Forum.Core.Models;

namespace Forum.Core.ActionResult
{
    public class NotFoundWithContent : ResultWithContent<List<ErrorDto>>
    {
        public NotFoundWithContent(HttpRequestMessage request, List<ErrorDto> errors) 
            : base(request, errors)
        {
        }

        protected override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.NotFound; }
        }
    }
}