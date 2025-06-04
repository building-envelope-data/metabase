using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionSortType
    : EntitySortType<Institution>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Institution> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Abbreviation);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.WebsiteLocator);
        descriptor.Field(x => x.State);
        descriptor.Field(x => x.Manager);
    }
}