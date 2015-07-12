using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Core.Helpers;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using MongoDB.Driver;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Should;

namespace Forum.IntegrationTests.Repositories
{
    public class SearchTermRepositoryTests
    {
        private readonly ISearchTermRepository repository;
        private const int NumberOfSearchTerms = 5;

        public SearchTermRepositoryTests()
        {
            var context = new Context();
            repository = context.Container.GetInstance<ISearchTermRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();
            
            var searchTerms = new List<SearchTerm>();
            fixture.RepeatCount = NumberOfSearchTerms;
            fixture.AddManyTo(searchTerms);

            CustomiseAndAddSampleSearchTerms(searchTerms);
        }

        [TearDown]
        public async void TearDown()
        {
            await repository.ClearCollection();
        }

        [Test]
        public async void GetAll_WhenCalledReturnsAllQuestions()
        {
            const int expectedNumber = NumberOfSearchTerms;

            var actual = await repository.GetAll();

            actual.Count.ShouldEqual(expectedNumber);
        }

        [Test]
        public async void GetById_WhenCalledWithExistingIdReturnsSearchTerm()
        {
            const string searchTermId = "2";
            const string expectedText = "search term two...";

            var actual = await repository.GetById(searchTermId);
            actual.Text.ShouldEqual(expectedText);
        }

        [Test]
        public async void GetById_WhenCalledWithNonExistingIdDoesNotReturnSearchTerm()
        {
            const string searchTermId = "99";

            var actual = await repository.GetById(searchTermId);
            actual.ShouldBeNull();
        }

        [Test]
        public async void GetByText_WhenTextMatchedReturnsQuestion()
        {
            const string text = "search term three...";
            const string expectedId = "3";

            var actual = await repository.GetByText(text);
            actual.Id.ShouldEqual(expectedId);
        }

        public async void Add_RemoveById_CanAddNewSearchTermThenRemoveByIdentifier()
        { }

        public async void Update_CanUpdateViewsByTextAndSeeUpdateApplied()
        { }

        public async void ReplaceById_CanReplaceByIdAndSeeReplacement()
        { }

        public async void ClearCollection_WhenCalled_CollectionIsEmptied()
        { }

        #region privates

        private void CustomiseAndAddSampleSearchTerms(IEnumerable<SearchTerm> searchTerms)
        {
            var identifier = 1;

            foreach (var searchTerm in searchTerms)
            {
                var localSearchTerm = searchTerm;
                localSearchTerm.Id = identifier.ToString(CultureInfo.InvariantCulture);
                localSearchTerm.Views = (identifier * 10);

                switch (identifier)
                {
                    case 1:
                        localSearchTerm.Text = "search term one...";
                        break;
                    case 2:
                        localSearchTerm.Text = "search term two...";
                        break;
                    case 3:
                        localSearchTerm.Text = "search term three...";
                        break;
                    case 4:
                        localSearchTerm.Text = "search term four...";
                        break;
                    case 5:
                        localSearchTerm.Text = "search term five...";
                        break;
                }

                identifier++;

                AsyncHelpers.RunSync(() => repository.Add(localSearchTerm));
            }

        }

        #endregion
    }
}
