using System;
using Forum.Domain.Extensions;
using Forum.Domain.Helpers;
using MongoDB.Bson;

namespace Forum.Domain.Entities
{
    public class QuestionAnswer
    {
        public QuestionAnswer()
        { }

        public QuestionAnswer(BsonValue bsonValue)
        {
            this.Id = bsonValue.TryGetString("_id", String.Empty);
            this.UserId = bsonValue.TryGetInt32("UserId", 0);
            this.DateTime = bsonValue.TryGetString("DateTime", String.Empty);
            this.DateTimeForMoment = bsonValue.TryGetString("DateTimeForMoment", String.Empty);
            this.Text = bsonValue.TryGetString("Text", String.Empty);
            this.CleanText = StringHelper.StripHTML(bsonValue.TryGetString("Text", String.Empty));
            this.Tags = bsonValue.TryGetString("Tags", String.Empty);
            this.Public = bsonValue.TryGetBoolean("Public", false);
            this.Removed = bsonValue.TryGetBoolean("Removed", false);
        }

        public string Id { get; set; }
        public string DateTime { get; set; }
        public string DateTimeForMoment { get; set; }
        public string Text { get; set; }
        public string CleanText { get; set; }
        public string Tags { get; set; }
        public int UserId { get; set; }
        public bool Public { get; set; }
        public bool Removed { get; set; }
        public bool IsBookmarked { get; set; }
    }
}
