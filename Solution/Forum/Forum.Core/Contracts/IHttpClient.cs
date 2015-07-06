using System;
using System.Collections.Generic;

namespace Forum.Core.Contracts
{
    public interface IHttpClient
    {
        string GetJson(string url, int timeoutInSeconds, IEnumerable<Tuple<string, string>> additionalHeaders = null);

        string PostJson(string url, string json, int timeoutInSeconds, IEnumerable<Tuple<string, string>> additionalHeaders = null);
    }
}
