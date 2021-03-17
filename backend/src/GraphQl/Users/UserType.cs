using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public sealed class UserType
      : EntityType<Data.User, UserByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.User> descriptor
            )
        {
            descriptor.BindFieldsExplicitly();
            base.Configure(descriptor);
            descriptor
              .Field(t => t.Email);
            descriptor
              .Field(t => t.Name);
            descriptor
              .Field(t => t.PhoneNumber);
            descriptor
              .Field(t => t.WebsiteLocator);
            descriptor
              .Field(t => t.DevelopedMethods);
            descriptor
                .Field(t => t.DevelopedMethods)
                .Type<NonNullType<ObjectType<UserDevelopedMethodConnection>>>()
                .Resolve(context =>
                    new UserDevelopedMethodConnection(
                        context.Parent<Data.User>()
                    )
                );
            descriptor
                .Field(t => t.RepresentedInstitutions)
                .Type<NonNullType<ObjectType<UserRepresentedInstitutionConnection>>>()
                .Resolve(context =>
                    new UserRepresentedInstitutionConnection(
                        context.Parent<Data.User>()
                    )
                );
        }
    }
}