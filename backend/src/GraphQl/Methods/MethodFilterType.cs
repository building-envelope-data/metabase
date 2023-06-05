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
            descriptor.Field(x => x.Standard);
            descriptor.Field(x => x.Publication);
            descriptor.Field(x => x.CalculationLocator);
            descriptor.Field(x => x.Categories);
            descriptor.Field(x => x.InstitutionDevelopers);
            descriptor.Field(x => x.UserDevelopers);
            descriptor.Field(x => x.Manager);
        }
    }
}