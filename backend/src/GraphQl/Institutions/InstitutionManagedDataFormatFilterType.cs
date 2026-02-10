using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.DataFormats;

public sealed class InstitutionManagedDataFormatFilterType
    : DataFormatFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<DataFormat> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedDataFormatFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Manager).Ignore();
    }
}