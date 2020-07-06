using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentPartInput
    {
        public Id AssembledComponentId { get; }
        public Id PartComponentId { get; }

        protected AddOrRemoveComponentPartInput(
            Id assembledComponentId,
            Id partComponentId
            )
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }
    }
}