using System.Linq;
using GraphQL.Types;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Services.GraphQL.Content.GraphTypes;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;

namespace GraphQL.SearchSchema.Conditions
{
    public class RootIdCondition : ISearchCondition
    {
        private ID GetValue(ResolveFieldContext context, Database database)
        {
            string inputPathOrIdOrShortId = context.GetArgument<string>("rootItem");
            ID rootId = null;
            Item result1;

            if (!string.IsNullOrWhiteSpace(inputPathOrIdOrShortId) &&
                IdHelper.TryResolveItem(database, inputPathOrIdOrShortId, out result1))
            {
                rootId = result1.ID;
            }

            return rootId;
        }

        public IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable)
        {
            ID rootId = GetValue(context, database);

            if (rootId != (ID) null)
            {
                queryable = queryable.Where(result => result.AncestorIDs.Contains(rootId));
            }

            return queryable;
        }
    }
}
