using System.Linq;
using GraphQL.SearchSchema.Conditions;
using GraphQL.Types;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using Sitecore.Services.GraphQL.Content;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;
using Sitecore.Services.GraphQL.GraphTypes.Connections;
using Sitecore.Services.GraphQL.Schemas;

namespace GraphQL.SearchSchema
{
    public class CustomSearchQuery : RootFieldType<CustomSearchResultsGraphType, CustomSearchResults>, IContentSchemaRootFieldType
    {
        public CustomSearchQuery() : base("customSearch", "Custom search query")
        {
            QueryArguments queryArguments = new QueryArguments();

            queryArguments.AddConnectionArguments();

            QueryArgument<StringGraphType> queryArgument1 = new QueryArgument<StringGraphType>
            {
                Name = "rootItem",
                Description = "ID or path of an item to search under (results will be descendants)"
            };

            queryArguments.Add(queryArgument1);

            QueryArgument<StringGraphType> queryArgument2 = new QueryArgument<StringGraphType>
            {
                Name = "keyword",
                Description = "Search by keyword (default: no keyword search)"
            };
            queryArguments.Add(queryArgument2);

            QueryArgument<StringGraphType> queryArgument3 = new QueryArgument<StringGraphType>
            {
                Name = "language", Description = "The item language to request (defaults to the context language)"
            };
            queryArguments.Add(queryArgument3);

            QueryArgument<BooleanGraphType> queryArgument4 = new QueryArgument<BooleanGraphType>
            {
                Name = "latestVersion",
                Description = "The item version to request (if not set, latest version is returned)",
                DefaultValue = true
            };
            queryArguments.Add(queryArgument4);

            QueryArgument<StringGraphType> queryArgument5 = new QueryArgument<StringGraphType>
            {
                Name = "index",
                Description =
                    "The search index name to query (defaults to the standard index for the current database)"
            };
            queryArguments.Add(queryArgument5);

            QueryArgument<ListGraphType<CustomSearchFieldQueryValueGraphType>> queryArgument6 = new QueryArgument<ListGraphType<CustomSearchFieldQueryValueGraphType>>
            {
                Name = "conditions",
                Description =
                    "Filter by index field value using equality (multiple fields are ANDed unless otherwise specified)"
            };
            queryArguments.Add(queryArgument6);

            QueryArgument<ListGraphType<NonNullGraphType<StringGraphType>>> queryArgument7 =
                new QueryArgument<ListGraphType<NonNullGraphType<StringGraphType>>>
                {
                    Name = "facetOn",
                    Description = "Index field names to facet results on"
                };
            queryArguments.Add(queryArgument7);

            Arguments = queryArguments;
        }

        protected override CustomSearchResults Resolve(ResolveFieldContext context)
        {
            string indexName = context.GetArgument<string>("index") ?? string.Format("sitecore_{0}_index", Database.Name.ToLowerInvariant());

            ISearchCondition[] conditions = {
                new RootIdCondition(), 
                new KeywordCondition(), 
                new LanguageCondition(), 
                new LatestVersionCondition(),
                new ConditionsCondition(), 
                new FacetOnCondition(), 
            };

            using (IProviderSearchContext searchContext = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                IQueryable<ContentSearchResult> queryable = searchContext.GetQueryable<ContentSearchResult>();

                foreach (ISearchCondition condition in conditions)
                {
                    queryable = condition.ApplyValue(context, Database, queryable);
                }

                int? afterValue = context.GetArgument<int?>("after", new int?());
                return new CustomSearchResults(queryable.ApplyEnumerableConnectionArguments(context).GetResults(), afterValue ?? 0);
            }
        }

        public Database Database { get; set; }
    }
}
