using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentInputType : InputObjectType<ComponentInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ComponentInput> descriptor)
        {
            descriptor.Name("ComponentInput");

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
