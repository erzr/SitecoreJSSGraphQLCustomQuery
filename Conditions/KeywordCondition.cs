using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Sitecore.Data;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;

namespace GraphQL.SearchSchema.Conditions
{
    public class KeywordCondition : ISearchCondition
    {
        private string GetValue(ResolveFieldContext context, Database database)
        {
            string keywordArg = context.GetArgument<string>("keyword", (string)null);
            return keywordArg;
        }

        public IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable)
        {
            string keywordArg = GetValue(context, database);

            if (!string.IsNullOrEmpty(keywordArg))
            {
                queryable = queryable.Where(result => result.Content.Contains(keywordArg));
            }

            return queryable;
        }
    }
}
