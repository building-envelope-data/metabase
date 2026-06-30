using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public abstract class InstitutionMethodDeveloperSortType
    : AuditableAssociationSortType<InstitutionMethodDeveloper>
{
    protected override void Configure(
        ISortInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
    }
}