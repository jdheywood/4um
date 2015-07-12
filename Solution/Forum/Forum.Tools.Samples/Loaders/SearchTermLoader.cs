using System.Collections.Generic;
using System.Globalization;
using Forum.Core.Helpers;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Ploeh.AutoFixture;

namespace Forum.Tools.Samples.Loaders
{
    public class SearchTermLoader
    {
        private const int NumberOfSearchTerms = 5;

        private readonly ISearchTermRepository searchTermRepository;
        
        public SearchTermLoader(ISearchTermRepository searchTermRepository)
        {
            this.searchTermRepository = searchTermRepository;
        }

        public async void SetUpSearchTerms()
        {
            var fixture = new Fixture();

            var searchTerms = new List<SearchTerm>();
            fixture.RepeatCount = NumberOfSearchTerms;
            fixture.AddManyTo(searchTerms);

            CustomiseAndAddSampleSearchTerms(searchTerms);
        }

        private void CustomiseAndAddSampleSearchTerms(IEnumerable<SearchTerm> searchTerms)
        {
            var identifier = 1;

            foreach (var searchTerm in searchTerms)
            {
                var localSearchTerm = searchTerm;
                searchTerm.Id = identifier.ToString(CultureInfo.InvariantCulture);
                searchTerm.Views = (identifier * 10);

                switch (identifier)
                {
                    case 1:
                        searchTerm.Text = "search term one...";
                        break;
                    case 2:
                        searchTerm.Text = "search term two...";
                        break;
                    case 3:
                        searchTerm.Text = "search term three...";
                        break;
                    case 4:
                        searchTerm.Text = "search term four...";
                        break;
                    case 5:
                        searchTerm.Text = "search term five...";
                        break;
                }

                identifier++;

                AsyncHelpers.RunSync(() => searchTermRepository.Add(localSearchTerm));
            }
        }
    }
}
