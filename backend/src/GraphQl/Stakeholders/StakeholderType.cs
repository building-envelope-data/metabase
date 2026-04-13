using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.Stakeholders;

public sealed class StakeholderType
    : UnionType<IStakeholder>
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Name(nameof(IStakeholder)[1..]);
    }
}