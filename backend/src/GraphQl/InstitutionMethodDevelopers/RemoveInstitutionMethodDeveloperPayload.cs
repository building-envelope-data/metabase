using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public sealed class RemoveInstitutionMethodDeveloperPayload
{
    private readonly InstitutionMethodDeveloper? _association;

    public RemoveInstitutionMethodDeveloperPayload(
        InstitutionMethodDeveloper institutionMethodDeveloper
    )
    {
        _association = institutionMethodDeveloper;
    }

    public RemoveInstitutionMethodDeveloperPayload(
        IReadOnlyCollection<RemoveInstitutionMethodDeveloperError> errors
    )
    {
        Errors = errors;
    }

    public RemoveInstitutionMethodDeveloperPayload(
        RemoveInstitutionMethodDeveloperError error
    )
        : this([error])
    {
    }

    public IReadOnlyCollection<RemoveInstitutionMethodDeveloperError>? Errors { get; }

    public async Task<Method?> GetMethodAsync(
        IMethodByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_association is null)
        {
            return null;
        }
        return await byId.LoadAsync(_association.MethodId, cancellationToken);
    }

    public async Task<Institution?> GetInstitutionAsync(
        IInstitutionByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_association is null)
        {
            return null;
        }
        return await byId.LoadAsync(_association.InstitutionId, cancellationToken);
    }
}