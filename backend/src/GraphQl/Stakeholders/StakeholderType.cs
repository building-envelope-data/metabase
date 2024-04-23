using HotChocolate.Types;

namespace Metabase.GraphQl.Stakeholders;

public sealed class StakeholderType
    : InterfaceType<Data.IStakeholder>
{
    protected override void Configure(IInterfaceTypeDescriptor<Data.IStakeholder> descriptor)
    {
        descriptor.Name(nameof(Data.IStakeholder)[1..]);
    }
}