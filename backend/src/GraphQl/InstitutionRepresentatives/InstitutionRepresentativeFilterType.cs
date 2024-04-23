using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed class InstitutionRepresentativeFilterType
    : FilterInputType<InstitutionRepresentative>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Institution);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Role);
        descriptor.Field(x => x.Pending);
    }
}