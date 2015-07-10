namespace Forum.UnitTests.TestObjects
{
    public class TestEmail
    {
        public TestEmail(string address)
        {
            Address = address;
        }

        public string Address { get; set; }
    }
}
