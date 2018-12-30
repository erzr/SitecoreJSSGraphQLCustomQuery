using System;
using System.Linq;
using GraphQL.Types;
using Sitecore.ContentSearch.Linq;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;
using Sitecore.Services.GraphQL.GraphTypes.Connections;
using Sitecore.Services.GraphQL.GraphTypes.Connections.DataObjects;

namespace GraphQL.SearchSchema
{
    public class CustomSearchResultsGraphType : ObjectGraphType<CustomSearchResults>
    {
        public CustomSearchResultsGraphType()
        {
            Name = "CustomSearchResults";
            Field<ConnectionGraphType<ContentSearchResultGraphType>>("results", null, null, ResolveResults);
            Field<ListGraphType<ContentSearchFacetGraphType>>("facets", null, null, ResolveFacets);
        }

        private object ResolveFacets(ResolveFieldContext<CustomSearchResults> context)
        {
            SearchHit<ContentSearchResult> searchHit = context.Source.Results.Hits.FirstOrDefault();
            string str;
            if (searchHit == null)
            {
                str = (string)null;
            }
            else
            {
                ContentSearchResult document = searchHit.Document;
                str = document != null ? document.DatabaseName : (string)null;
            }
            string db = str;
            return (object)context.Source.Results.Facets.Categories.Select(category => new ContentSearchFacetGraphType.ItemSearchFacetGraphTypeModel(category, db));
        }

        private object ResolveResults(ResolveFieldContext<CustomSearchResults> context)
        {
            return new EnumerableConnection<SearchHit<ContentSearchResult>>(context.Source.Results.Hits, context.Source.Results.TotalSearchResults, context.Source.After);
        }
    }
}
