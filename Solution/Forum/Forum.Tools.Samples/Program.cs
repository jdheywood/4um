using System;
using Forum.Domain.Contracts;
using Forum.Tools.Samples.Loaders;
using Forum.Tools.Samples.Removers;
using SimpleInjector;

namespace Forum.Tools.Samples
{
    public class Program
    {
        public static Container Container { get; set; }
        public static IMongoContext MongoContext { get; set; }
        private static IQuestionRepository questionRepository;
        private static IAnswerRepository answerRepository;
        private static ISearchTermRepository searchTermRepository;

        public static void Main(string[] args)
        {
            SetUpcontainerAndContext();

            questionRepository = Container.GetInstance<IQuestionRepository>();
            answerRepository = Container.GetInstance<IAnswerRepository>();
            searchTermRepository = Container.GetInstance<ISearchTermRepository>();

            Console.WriteLine("Mongo document samples manager");

            var remove = false;
            CheckArgs(args, out remove);

            if (remove)
            {
                Console.WriteLine("Removing sample documents...");

                var questionRemover = new QuestionRemover(questionRepository);
                questionRemover.Remove();

                var answerRemover = new AnswerRemover(answerRepository);
                answerRemover.Remove();

                var searchTermRemover = new SearchTermRemover(searchTermRepository);
                searchTermRemover.Remove();
            }
            else
            {
                // TODO make the load dependent on an empty database to prevent duplicate id exceptions

                var questionLoader = new QuestionLoader(questionRepository);
                questionLoader.SetUpQuestions();

                var answerLoader = new AnswerLoader(answerRepository, questionRepository);
                answerLoader.SetUpAnswers();

                var searchTermLoader = new SearchTermLoader(searchTermRepository);
                searchTermLoader.SetUpSearchTerms();
            }

            Console.WriteLine("finished, press any key to close...");
            Console.ReadKey();
        }

        #region Privates

        private static void SetUpcontainerAndContext()
        {
            Container = new Container();

            SamplesContainer.RegisterDependencies(Container);

            Container.Verify();

            MongoContext = Container.GetInstance<IMongoContext>();
        }

        private static void CheckArgs(string[] args, out bool remove)
        {
            remove = false;

            if (args.Length <= 0) return;

            if (args[0] == "remove")
                remove = true;
        }

        #endregion
    }
}
