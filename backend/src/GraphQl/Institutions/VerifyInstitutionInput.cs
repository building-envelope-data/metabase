using System;

namespace Metabase.GraphQl.Institutions
{
    public sealed record VerifyInstitutionInput(
        Guid InstitutionId
    );
}