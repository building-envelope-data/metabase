using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Databases;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabaseSortType
    : DatabaseSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionOperatedDatabaseSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}