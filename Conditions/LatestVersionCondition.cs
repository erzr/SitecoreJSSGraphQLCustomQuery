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
    public class LatestVersionCondition : ISearchCondition
    {
        private bool GetValue(ResolveFieldContext context)
        {
            bool? nullable1 = context.GetArgument<bool?>("version", new bool?());
            bool flag = !nullable1.HasValue || nullable1.GetValueOrDefault();
            return flag;
        }

        public IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable)
        {
            bool shouldEnable = GetValue(context);

            if (shouldEnable)
            {
                queryable = queryable.Where(result => result.IsLatestVersion);
            }

            return queryable;
        }
    }
}
