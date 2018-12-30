using GraphQL.Types;

namespace GraphQL.SearchSchema
{
    public class CustomSearchFieldQueryValueGraphType : InputObjectGraphType
    {
        public CustomSearchFieldQueryValueGraphType()
        {
            Name = "CustomItemSearchFieldQuery";
            Field<StringGraphType>("name", "Index field name to filter on");
            Field<StringGraphType>("value", "Field value to filter on");
            Field<BooleanGraphType>("useor", "If this condition should be Or'ed with the previous");
            Field<ListGraphType<CustomSearchFieldQueryValueGraphType>>("group", "Nested conditions");
        }
    }
}