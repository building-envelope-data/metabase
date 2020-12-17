namespace Metabase.GraphQl.Components
{
  public sealed class CreateComponentPayload
    : ComponentPayload
    {
      public CreateComponentPayload(
          Data.Component component
          )
            : base(component)
        {
        }
    }
}
