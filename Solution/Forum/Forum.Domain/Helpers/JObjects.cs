using System;
using Newtonsoft.Json.Linq;

namespace Forum.Domain.Helpers
{
    public static class JObjects
    {
        public static JToken User(this JObject json)
        {
            return json["user"];
        }

        public static JToken Email(this JObject json)
        {
            return json["email"];
        }

        public static JToken Sms(this JObject json)
        {
            return json["sms"];
        }

        public static JToken Content(this JObject json)
        {
            return json["content"];
        }

        public static string MessageType(this JToken token)
        {
            return (string)token["type"];
        }

        public static int Id(this JToken token)
        {
            return (int)token["id"];
        }

        public static string Name(this JToken token)
        {
            return (string)token["name"];
        }

        public static string Subject(this JToken token)
        {
            return (string)token["subject"];
        }

        public static string Body(this JToken token)
        {
            return (string)token["body"];
        }

        public static string Template(this JToken token)
        {
            return (string)token["template"];
        }

        public static string Link(this JToken token)
        {
            return (string)token["link"];
        }

        public static string Key(this JToken token)
        {
            return (string)token["key"];
        }

        public static string Language(this JToken token)
        {
            return (string)token["language"];
        }

        public static string Type(this JToken token)
        {
            return (string)token["type"];
        }

        public static string Address(this JToken token)
        {
            return (string)token["address"];
        }

        public static string Number(this JToken token)
        {
            return (string)token["number"];
        }

        public static string Originator(this JToken token)
        {
            return (string)token["originator"];
        }
        
        public static string Description(this JToken token)
        {
            return (string)token["description"];
        }

        public static string Examples(this JToken token)
        {
            return (string)token["examples"];
        }

        public static String Images(this JToken token)
        {
            return (string)token["images"];
        }

        public static string PasswordChangeKey(this JObject json)
        {
            return (string)json["passwordChangeKey"];
        }        
    }
}
