namespace Metabase.GraphQl.Methods
{
    public sealed class CreateMethodPayload
      : MethodPayload<CreateMethodError>
    {
        public CreateMethodPayload(
            Data.Method method
            )
              : base(method)
        {
        }
    }
}