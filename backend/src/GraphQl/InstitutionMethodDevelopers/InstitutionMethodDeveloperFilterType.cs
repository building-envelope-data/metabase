using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed class InstitutionMethodDeveloperFilterType
      : FilterInputType<Data.InstitutionMethodDeveloper>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.InstitutionMethodDeveloper> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Method);
            descriptor.Field(x => x.Institution);
            descriptor.Field(x => x.Pending);
        }
    }
}