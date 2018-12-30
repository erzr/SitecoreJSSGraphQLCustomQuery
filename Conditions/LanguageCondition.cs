using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Services.GraphQL.Content.GraphTypes.ContentSearch;

namespace GraphQL.SearchSchema.Conditions
{
    public class LanguageCondition : ISearchCondition
    {
        private Sitecore.Globalization.Language GetValue(ResolveFieldContext context)
        {
            string languageParameter = context.GetArgument<string>("language");

            Sitecore.Globalization.Language result2;
            if (!Sitecore.Globalization.Language.TryParse(
                languageParameter ??
                Context.Language.Name ?? LanguageManager.DefaultLanguage.Name, out result2))
            {
                result2 = null;
            }

            return result2;
        }

        public IQueryable<ContentSearchResult> ApplyValue(ResolveFieldContext context, Database database, IQueryable<ContentSearchResult> queryable)
        {
            Sitecore.Globalization.Language language = GetValue(context);

            if (language != null)
            {
                string resultLanguage = language.Name;
                queryable = queryable.Where(result => result.Language == resultLanguage);
            }

            return queryable;
        }
    }
}
