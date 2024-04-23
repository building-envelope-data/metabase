using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.Stakeholders;

public sealed class StakeholderType
    : InterfaceType<IStakeholder>
{
    protected override void Configure(IInterfaceTypeDescriptor<IStakeholder> descriptor)
    {
        descriptor.Name(nameof(IStakeholder)[1..]);
    }
}