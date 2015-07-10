using Forum.Domain.Helpers;
using Forum.UnitTests.TestObjects;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Should;

namespace Forum.UnitTests.Helpers
{
    public class JObjectsTests
    {
        private JObject json;

        private const string UserName = "James";
        private const string EmailAddress = "james@email.com";
        private const string SmsMessage = "hello world";

        [SetUp]
        public void SetUp()
        {
            json = new JObject();

            var userValue = JToken.FromObject(new TestUser(UserName));
            var emailValue = JToken.FromObject(new TestEmail(EmailAddress));
            var smsValue = JToken.FromObject(new TestSms(SmsMessage));
            
            json.Add("email", emailValue);
            json.Add("sms", smsValue);
            json.Add("user", userValue);

        }

        [Test]
        public void User_ReturnsUserPropertyFromJObject()
        {
            var result = json.User();

            var actual = result.Value<string>("Name");

            actual.ShouldEqual(UserName);
        }

        [Test]
        public void Email_ReturnsEmailPropertyFromJObject()
        {
            var result = json.Email();

            var actual = result.Value<string>("Address");

            actual.ShouldEqual(EmailAddress);
        }

        [Test]
        public void Sms_ReturnsSmsPropertyFromJObject()
        {
            var result = json.Sms();

            var actual = result.Value<string>("Message");

            actual.ShouldEqual(SmsMessage);
        }

        // TODO complete test coverage for JObjects helper

    }
}
