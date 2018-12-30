using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.Linq;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;

namespace GraphQL.SearchSchema
{
    public class CustomSearchResults
    {
        public SearchResults<ContentSearchResult> Results { get; }

        public int After { get; set; }

        public CustomSearchResults(SearchResults<ContentSearchResult> results, int after)
        {
            Results = results;
            After = after;
        }
    }
}
