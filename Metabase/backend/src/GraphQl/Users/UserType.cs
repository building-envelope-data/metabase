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
        }
    }
}