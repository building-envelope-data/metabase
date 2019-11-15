using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentInformationType : ObjectType<ComponentInformation>
    {
        protected override void Configure(IObjectTypeDescriptor<ComponentInformation> descriptor)
        {
            descriptor.Name("ComponentInformation");

            descriptor.Field(t => t.Name)
              .Type<NonNullType<StringType>>();

            descriptor.Field(t => t.Abbreviation)
              .Type<StringType>();

            descriptor.Field(t => t.Description)
              .Type<NonNullType<StringType>>();

            descriptor.Field(t => t.AvailableFrom)
              .Type<DateTimeType>();

            descriptor.Field(t => t.AvailableUntil)
              .Type<DateTimeType>();

            descriptor.Field(t => t.Categories)
              .Type<NonNullType<ListType<NonNullType<ComponentCategoryType>>>>();
        }
    }
}
