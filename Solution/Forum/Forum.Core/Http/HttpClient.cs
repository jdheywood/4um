using System;
using System.Collections.Generic;
using System.Net;
using Forum.Core.Contracts;
using Forum.Core.Web;

namespace Forum.Core.Http
{
    public class HttpClient : IHttpClient
    {
        private const string JsonMimeType = "application/json";

        public string GetJson(string url, int timeoutInSeconds, IEnumerable<Tuple<string, string>> additionalHeaders = null)
        {
            string response;

            using (var webClient = new CustomWebClient(timeoutInSeconds))
            {
                webClient.Headers.Add(HttpRequestHeader.Accept, JsonMimeType);

                SetAdditionalHeaders(additionalHeaders, webClient);

                response = webClient.DownloadString(url);
            }

            return response;
        }

        public string PostJson(string url, string json, int timeoutInSeconds, IEnumerable<Tuple<string, string>> additionalHeaders = null)
        {
            string response;

            using (var webClient = new CustomWebClient(timeoutInSeconds))
            {
                webClient.Headers.Add(HttpRequestHeader.ContentType, JsonMimeType);

                SetAdditionalHeaders(additionalHeaders, webClient);

                response = webClient.UploadString(url, json);
            }

            return response;
        }

        private static void SetAdditionalHeaders(IEnumerable<Tuple<string, string>> additionalHeaders, WebClient webClient)
        {
            if (additionalHeaders == null)
            {
                return;
            }

            foreach (var header in additionalHeaders)
            {
                webClient.Headers.Add(header.Item1, header.Item2);
            }
        }
    }
}
