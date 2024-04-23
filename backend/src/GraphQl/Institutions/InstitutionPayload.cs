using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public abstract class InstitutionPayload<TInstitutionError>
    : Payload
    where TInstitutionError : IUserError
{
    public Data.Institution? Institution { get; }
    public IReadOnlyCollection<TInstitutionError>? Errors { get; }

    protected InstitutionPayload(
        Data.Institution institution
    )
    {
        Institution = institution;
    }

    protected InstitutionPayload(
        IReadOnlyCollection<TInstitutionError> errors
    )
    {
        Errors = errors;
    }

    protected InstitutionPayload(
        TInstitutionError error
    )
        : this(new[] { error })
    {
    }

    protected InstitutionPayload(
        Data.Institution institution,
        IReadOnlyCollection<TInstitutionError> errors
    )
    {
        Institution = institution;
        Errors = errors;
    }

    protected InstitutionPayload(
        Data.Institution institution,
        TInstitutionError error
    )
        : this(
            institution,
            new[] { error }
        )
    {
    }
}