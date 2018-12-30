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
    public interface ISearchCondition
    {
        IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable);
    }
}
