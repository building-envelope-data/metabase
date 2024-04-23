using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Users
{
    public sealed class UserFilterType
        : FilterInputType<Data.User>
    {
        protected override void Configure(
            IFilterInputTypeDescriptor<Data.User> descriptor
        )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Id);
            // TODO The commented fiels below should be filterable by OpenId Connect Clients and application users with the proper scopes and rights. If they are filterable in general, it is a way to figure out that information even if it is not returned by GraphQL.
            // descriptor.Field(x => x.Name);
            // descriptor.Field(x => x.Email);
            // descriptor.Field(x => x.PostalAddress);
            // descriptor.Field(x => x.WebsiteLocator);
            descriptor.Field(x => x.DevelopedMethods);
            descriptor.Field(x => x.DevelopedMethodEdges);
            descriptor.Field(x => x.RepresentedInstitutions);
            descriptor.Field(x => x.RepresentedInstitutionEdges);
        }
    }
}