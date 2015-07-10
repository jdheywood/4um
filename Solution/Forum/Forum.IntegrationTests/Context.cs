using Forum.Domain.Contracts;
using SimpleInjector;

namespace Forum.IntegrationTests
{
    public class Context
    {
        public Container Container { get; set; }
        public IMongoContext MongoContext { get; set; }

        public Context()
        {
            Container = new Container();

            TestContainer.RegisterDependencies(Container);

            Container.Verify();

            MongoContext = Container.GetInstance<IMongoContext>();
        }
    }
}
