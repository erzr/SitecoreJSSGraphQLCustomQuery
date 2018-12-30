using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;

namespace GraphQL.SearchSchema.Conditions
{
    public class ConditionsCondition : ISearchCondition
    {
        private Dictionary<string, object>[] GetValue(ResolveFieldContext context)
        {
            if (context.HasArgument("conditions"))
            {
                IEnumerable<Dictionary<string, object>> dictionaries =
                    (context.Arguments["conditions"] as object[] ?? new object[0]).OfType<Dictionary<string, object>>();
                return dictionaries.ToArray();
            }

            return null;
        }

        public IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable)
        {
            Dictionary<string, object>[] dictionaries = GetValue(context);

            var expression = ApplyValue(queryable, dictionaries);

            queryable = queryable.Where(expression);

            return queryable;
        }

        public Expression<Func<ContentSearchResult, bool>> ApplyValue(IQueryable<ContentSearchResult> queryable, Dictionary<string, object>[] dictionaries)
        {
            var predicateBuilder = PredicateBuilder.True<ContentSearchResult>();

            if (dictionaries != null && dictionaries.Any())
            {
                foreach (Dictionary<string, object> dictionary in dictionaries)
                {
                    string name = dictionary.ContainsKey("name") ? dictionary["name"].ToString() : null;
                    string value = dictionary.ContainsKey("value") ? dictionary["value"].ToString() : null;
                    bool? useOr = dictionary.ContainsKey("useor") ? dictionary["useor"] as bool? : false;
                    Dictionary<string, object>[] group = (dictionary.ContainsKey("group") ? dictionary["group"] as object[] : new object[0]).OfType<Dictionary<string, object>>().ToArray();

                    if (group.Any())
                    {
                        if (useOr.HasValue && useOr.Value)
                        {
                            predicateBuilder = predicateBuilder.Or(ApplyValue(queryable, group));
                        }
                        else
                        {
                            predicateBuilder = predicateBuilder.And(ApplyValue(queryable, group));
                        }
                    }
                    else
                    {
                        if (useOr.HasValue && useOr.Value)
                        {
                            predicateBuilder = predicateBuilder.Or(result => result[name].Equals(value));
                        }
                        else
                        {
                            predicateBuilder = predicateBuilder.And(result => result[name].Equals(value));
                        }
                    }
                }
            }

            return predicateBuilder;
        }
    }
}
