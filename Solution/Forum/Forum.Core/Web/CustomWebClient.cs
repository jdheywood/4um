using System;
using System.Net;

namespace Forum.Core.Web
{
    public class CustomWebClient : WebClient
    {
        private readonly int timeoutInSeconds;

        public CustomWebClient(int timeoutInSeconds)
        {
            this.timeoutInSeconds = timeoutInSeconds;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = (HttpWebRequest) base.GetWebRequest(address);
            if (webRequest != null)
            {
                webRequest.Timeout = timeoutInSeconds*1000;
                webRequest.KeepAlive = false;
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.AllowWriteStreamBuffering = false;
            }
            return webRequest;
        }
    }
}
