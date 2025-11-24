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
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Abbreviation);
        descriptor.Field(_ => _.Description);
        descriptor.Field(_ => _.Contact);
        descriptor.Field(_ => _.State);
        descriptor.Field(_ => _.Manager);
    }
}