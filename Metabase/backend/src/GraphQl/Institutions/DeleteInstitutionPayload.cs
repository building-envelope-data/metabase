using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public sealed class DeleteInstitutionPayload
    {
        public IReadOnlyCollection<DeleteInstitutionError>? Errors { get; }

        public DeleteInstitutionPayload()
        {
        }

        public DeleteInstitutionPayload(
            DeleteInstitutionError error
            )
        {
            Errors = new[] { error };
        }
    }
}