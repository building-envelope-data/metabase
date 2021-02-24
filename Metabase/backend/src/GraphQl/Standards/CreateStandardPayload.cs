namespace Metabase.GraphQl.Standards
{
    public sealed class CreateStandardPayload
      : StandardPayload<CreateStandardError>
    {
        public CreateStandardPayload(
            Data.Standard standard
            )
              : base(standard)
        {
        }
    }
}