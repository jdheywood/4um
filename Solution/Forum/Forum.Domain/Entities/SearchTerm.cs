using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Forum.Domain.Entities
{
    public class SearchTerm : Entity
    {
        public SearchTerm()
        {
        }

        public SearchTerm(string jsonData)
        {
            this.Id = Guid.NewGuid().ToString();

            JObject jObject = JObject.Parse(jsonData);
            IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();

            if (keys.Contains("text"))
                this.Text = (string)jObject["text"];

            this.Views = 1;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public int Views { get; set; }
    }
}
