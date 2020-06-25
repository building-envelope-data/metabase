namespace Icon.GraphQl
{
    public sealed class RemoveComponentPartPayload
      : AddOrRemoveComponentPartPayload
    {
        public RemoveComponentPartPayload(
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId,
            ValueObjects.Timestamp requestTimestamp
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