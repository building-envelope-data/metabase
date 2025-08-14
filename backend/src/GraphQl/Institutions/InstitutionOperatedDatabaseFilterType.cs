using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.Databases;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabaseFilterType
    : DatabaseFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionOperatedDatabaseFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.Operator).Ignore();
    }
}