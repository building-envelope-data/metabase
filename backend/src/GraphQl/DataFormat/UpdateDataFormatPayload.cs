namespace Metabase.GraphQl.DataFormats
{
    public sealed class UpdateDataFormatPayload
        : DataFormatPayload<UpdateDataFormatError>
    {
        public UpdateDataFormatPayload(
            Data.DataFormat standard
        )
            : base(standard)
        {
        }

        public UpdateDataFormatPayload(
            UpdateDataFormatError error
        )
            : base(error)
        {
        }
    }
}