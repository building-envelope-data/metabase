using Metabase.Data;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class CreateApplicationPayload
    : ApplicationPayload<CreateApplicationError>
{
    public string? ClientSecret { get; }

    public CreateApplicationPayload(
        OpenIdApplication application,
        string clientSecret
    )
        : base(application)
    {
        ClientSecret = clientSecret;
    }

    public CreateApplicationPayload(
        CreateApplicationError error
    )
        : base(error)
    {
    }
}