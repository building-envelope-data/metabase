namespace Icon.GraphQl
{
    public sealed class RemoveComponentVersionPayload
      : AddOrRemoveComponentVersionPayload
    {
        public RemoveComponentVersionPayload(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              baseComponentId: baseComponentId,
              versionComponentId: versionComponentId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}