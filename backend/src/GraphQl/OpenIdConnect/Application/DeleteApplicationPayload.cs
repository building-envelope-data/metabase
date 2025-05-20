using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class DeleteApplicationPayload
{
    public DeleteApplicationPayload()
    {
    }

    public DeleteApplicationPayload(
        DeleteApplicationError error
    )
    {
        Errors = [error];
    }

    public IReadOnlyCollection<DeleteApplicationError>? Errors { get; }
}