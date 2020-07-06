using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveComponentPartPayload
      : AddOrRemoveComponentPartPayload
    {
        public RemoveComponentPartPayload(
            Id assembledComponentId,
            Id partComponentId,
            Timestamp requestTimestamp
            )
          : base(
              assembledComponentId: assembledComponentId,
              partComponentId: partComponentId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}