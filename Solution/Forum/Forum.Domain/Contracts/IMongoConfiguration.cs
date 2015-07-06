namespace Forum.Domain.Contracts
{
    public interface IMongoConfiguration
    {
        string ConnectionString { get; }

        string DatabaseName { get; }

        string AnswerCollectionName { get; }

        string QuestionCollectionName { get; }

        string SearchTermCollectionName { get; }
    }
}
