using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.UserMethodDevelopers;

public abstract class UserMethodDeveloperFilterType
    : AuditableAssociationFilterType<UserMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<UserMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.Method);
        descriptor.Field(_ => _.User);
    }
}
