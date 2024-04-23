using System;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public sealed record RemoveInstitutionMethodDeveloperInput(
    Guid MethodId,
    Guid InstitutionId
);