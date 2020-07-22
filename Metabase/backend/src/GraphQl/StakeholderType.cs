using HotChocolate.Types;

namespace Metabase.GraphQl
{
    public sealed class StakeholderType
      : UnionType<IStakeholder>
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor.Name(typeof(IStakeholder).Name.Substring(1));
        }
    }
}