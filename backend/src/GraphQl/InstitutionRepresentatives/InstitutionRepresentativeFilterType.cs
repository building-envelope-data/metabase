using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class InstitutionRepresentativeFilterType
      : FilterInputType<Data.InstitutionRepresentative>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.InstitutionRepresentative> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Institution);
            descriptor.Field(x => x.User);
            descriptor.Field(x => x.Role);
            descriptor.Field(x => x.Pending);
        }
    }
}