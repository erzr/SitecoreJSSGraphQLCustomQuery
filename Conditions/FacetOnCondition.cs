using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;

namespace GraphQL.SearchSchema.Conditions
{
    public class FacetOnCondition : ISearchCondition
    {
        public IEnumerable<string> GetValue(ResolveFieldContext context)
        {
            IEnumerable<string> strings =
                (IEnumerable<string>)((object)context.GetArgument("facetOn", (IEnumerable<string>)null) ??
                                      (object)new string[0]);
            return strings;
        }

        public IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable)
        {
            IEnumerable<string> facetFields = GetValue(context);

            foreach (string str in facetFields)
            {
                string facet = str;
                queryable = queryable.FacetOn(result => result[facet]);
            }

            return queryable;
        }
    }
}
