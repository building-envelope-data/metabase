namespace Metabase.GraphQl.DataFormats
{
    public sealed class CreateDataFormatPayload
        : DataFormatPayload<CreateDataFormatError>
    {
        public CreateDataFormatPayload(
            Data.DataFormat standard
        )
            : base(standard)
        {
        }

        public CreateDataFormatPayload(
            CreateDataFormatError error
        )
            : base(error)
        {
        }
    }
}