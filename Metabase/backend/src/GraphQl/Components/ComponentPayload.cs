namespace Metabase.GraphQl.Components
{
  public abstract class ComponentPayload
    : GraphQl.Payload
    {
        public Data.Component Component { get; }

        protected ComponentPayload(
            Data.Component component
            )
        {
            Component = component;
        }
    }
}
