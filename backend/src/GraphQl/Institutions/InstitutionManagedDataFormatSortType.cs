using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.DataFormats;

public sealed class InstitutionManagedDataFormatSortType
    : DataFormatSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<DataFormat> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedDataFormatSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}