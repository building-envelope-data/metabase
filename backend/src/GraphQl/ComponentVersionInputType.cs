using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentVersionInputType : InputObjectType<ComponentVersionInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ComponentVersionInput> descriptor)
        {
            descriptor.Name("ComponentVersionInput");

            descriptor.Field(t => t.ComponentId)
                .Type<NonNullType<UuidType>>();

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