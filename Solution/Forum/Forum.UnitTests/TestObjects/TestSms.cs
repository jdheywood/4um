namespace Forum.UnitTests.TestObjects
{
    public class TestSms
    {
        public TestSms(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
