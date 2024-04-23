namespace Metabase.GraphQl.Methods
{
    public sealed class UpdateMethodPayload
        : MethodPayload<UpdateMethodError>
    {
        public UpdateMethodPayload(
            Data.Method method
        )
            : base(method)
        {
        }

        public UpdateMethodPayload(
            UpdateMethodError error
        )
            : base(error)
        {
        }
    }
}