using Forum.Domain.Helpers;
using NUnit.Framework;
using Should;

namespace Forum.UnitTests.Helpers
{
    public class StringHelperTests
    {
        private const string MessyCaseString = "tHiS iS aN eXaMpLE sTrInG 123 !<>`'#~";
        private const string ProperCaseString = "This is an example string 123 !<>`'#~";
        private const string UrlString = "this-is-an-example-string-123";
        private const string TruncatedString = "tHiS iS aN eXaMpLE...";

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void ProperCase_WhenCalledProperCasesInputAsExpected()
        {
            var result = StringHelper.ProperCase(MessyCaseString);

            result.ShouldEqual(ProperCaseString);
        }

        [Test]
        public void GenerateUrl_WhenCalledReturnsStringSuitableForUseAsUrl()
        {
            var result = StringHelper.GenerateURL(MessyCaseString);

            result.ShouldEqual(UrlString);
        }

        [Test]
        public void TruncateString_WhenPassedStringAndLengthReturnsExpected()
        {
            var result = StringHelper.TruncateString(MessyCaseString, 18);

            result.ShouldEqual(TruncatedString);
        }

        // TODO complete test coverage of StringHelper

    }
}
