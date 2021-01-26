using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodFilterType
      : FilterInputType<Data.Method>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.Method> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Id);
            descriptor.Field(x => x.Name);
            descriptor.Field(x => x.Description);
            descriptor.Field(x => x.PublicationLocator);
            descriptor.Field(x => x.CodeLocator);
            descriptor.Field(x => x.Categories);
        }
    }
}