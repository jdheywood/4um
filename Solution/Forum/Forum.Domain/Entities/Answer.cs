using System;
using System.Collections.Generic;
using System.Linq;
using Forum.Domain.Helpers;
using Newtonsoft.Json.Linq;

namespace Forum.Domain.Entities
{
    public class Answer : Entity
    {
        public Answer()
        { }

        public Answer(string jsonData)
        {
            this.Id = Guid.NewGuid().ToString();

            JObject jObject = JObject.Parse(jsonData);
            IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();

            if (keys.Contains("questionid"))
                this.QuestionId = (string)jObject["questionid"];

            if (keys.Contains("userid"))
                this.UserId = (int)jObject["userid"];

            if (keys.Contains("text"))
            {
                this.Text = (string)jObject["text"];
                this.CleanText = StringHelper.StripHTML(((string)jObject["text"]).Replace("><", "> <"));
            }

            if (keys.Contains("tags"))
            {
                string s = (string)jObject["tags"];
                this.Tags = s.Split(',');
            }

            if (keys.Contains("views"))
                this.Views = (int)jObject["views"];

            if (keys.Contains("public"))
                this.Public = (bool)jObject["public"];

            if (keys.Contains("removed"))
                this.Public = (bool)jObject["removed"];
        }

        public string Id { get; set; }

        public string QuestionId { get; set; }
        public int UserId { get; set; }

        public DateTime? DateTime { get; set; }
        public string Text { get; set; }
        public string CleanText { get; set; }
        public string[] Tags { get; set; }
        public int Views { get; set; }

        public bool Public { get; set; }
        public bool Removed { get; set; }

        #region Helpers

        public string NiceDateTime
        {
            get
            {
                if (DateTime == null)
                {
                    return "";
                }
                else
                {
                    var tz = TimeZone.CurrentTimeZone;
                    var tzi = TimeZoneInfo.FindSystemTimeZoneById(tz.StandardName);
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Value.ToUniversalTime(), tzi).ToString("HH:mm dd-MM-yyyy");
                }
            }
        }

        public string DateTimeForMoment
        {
            get
            {
                if (DateTime == null)
                {
                    return "";
                }
                else
                {
                    var tz = TimeZone.CurrentTimeZone;
                    var tzi = TimeZoneInfo.FindSystemTimeZoneById(tz.StandardName);
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Value.ToUniversalTime(), tzi).ToString("yyyy MM dd H:mm");
                }
            }
        }

        #endregion
    }
}
