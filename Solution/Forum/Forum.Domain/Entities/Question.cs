using System;
using System.Collections.Generic;
using System.Linq;
using Forum.Domain.Extensions;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace Forum.Domain.Entities
{
    public class Question : Entity
    {
        public Question()
        { }

        public Question(string jsonData)
        {
            this.Id = Guid.NewGuid().ToString();

            JObject jObject = JObject.Parse(jsonData);
            IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();

            if (keys.Contains("askedby"))
                this.UserIdAsked = (int)jObject["askedby"];

            if (keys.Contains("answeredby"))
                this.UserIdAnswered = (int)jObject["answeredby"];

            if (keys.Contains("askedbyname"))
                this.AskedByName = (string)jObject["askedbyname"];

            if (keys.Contains("askedbyemail"))
                this.AskedByName = (string)jObject["askedbyemail"];

            if (keys.Contains("askedbyphone"))
                this.AskedByName = (string)jObject["askedbyphone"];

            if (keys.Contains("text"))
                this.Text = (string)jObject["text"];

            if (keys.Contains("area"))
                this.Area = (string)jObject["area"];

            if (keys.Contains("views"))
                this.Views = (int)jObject["views"];

            if (keys.Contains("approved"))
                this.Approved = (bool)jObject["approved"];

            if (keys.Contains("archived"))
                this.Archived = (bool)jObject["archived"];

            if (keys.Contains("removed"))
                this.Removed = (bool)jObject["removed"];

            if (keys.Contains("replied"))
                this.Replied = (bool)jObject["replied"];
        }

        public Question(BsonValue bsonValue)
        {
            this.Id = bsonValue.TryGetString("_id", String.Empty);
            this.UserIdAsked = bsonValue.TryGetInt32("UserIdAsked", 0);
            this.UserIdAnswered = bsonValue.TryGetInt32("UserIdAnswered", 0);
            this.AskedByName = bsonValue.TryGetString("AskedByName", String.Empty);
            this.AskedByEmail = bsonValue.TryGetString("AskedByEmail", String.Empty);
            this.AskedByPhone = bsonValue.TryGetString("AskedByPhone", String.Empty);
            this.DateTime = bsonValue.TryGetNullableDateTime("DateTime", new Nullable<DateTime>());
            this.Text = bsonValue.TryGetString("Text", String.Empty);
            this.Area = bsonValue.TryGetString("Area", String.Empty);
            this.Views = bsonValue.TryGetInt32("Views", 0);
            this.Approved = bsonValue.TryGetBoolean("Approved", false);
            this.Archived = bsonValue.TryGetBoolean("Archived", false);
            this.Removed = bsonValue.TryGetBoolean("Removed", false);
            this.Replied = bsonValue.TryGetBoolean("Replied", false);
        }

        public string Id { get; set; }

        public int UserIdAsked { get; set; }
        public int UserIdAnswered { get; set; }
        
        public string AskedByName { get; set; }
        public string AskedByEmail { get; set; }
        public string AskedByPhone { get; set; }

        public DateTime? DateTime { get; set; }
        public string Text { get; set; }
        public string Area { get; set; }
        public int Views { get; set; }

        public bool Approved { get; set; }
        public bool Archived { get; set; }
        public bool Removed { get; set; }
        public bool Replied { get; set; }

        public QuestionAnswer[] Answers { get; set; }


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

        #endregion
    }
}
