using HotChocolate.Types;
/* using HotChocolate.AspNetCore.Authorization; */

namespace Icon.GraphQl
{
    public class QueryType
        : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(t => t.GetComponents(default, default))
                .Type<NonNullType<ListType<NonNullType<ComponentType>>>>()
                .Argument("timestamp", a => a.Type<DateTimeType>());

            descriptor.Field(t => t.GetComponent(default, default, default))
                .Type<NonNullType<ComponentType>>()
                .Argument("id", a => a.Type<NonNullType<UuidType>>())
                .Argument("timestamp", a => a.Type<DateTimeType>());

            /* descriptor.Field(t => t.GetCharacter(default, default)) */
            /*     .Type<NonNullType<ListType<NonNullType<CharacterType>>>>(); */

            // the search can only be executed if the current
            // identity has a country
            /* descriptor.Field(t => t.Search(default)) */
            /*     .Type<ListType<SearchResultType>>() */
            /*     .Authorize("HasCountry"); */
        }
    }
}