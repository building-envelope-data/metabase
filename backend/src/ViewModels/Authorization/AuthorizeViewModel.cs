using System.ComponentModel.DataAnnotations;
using Metabase.GraphQl.OpenIdConnect;

namespace Metabase.ViewModels.Authorization;

public sealed record AuthorizeViewModel(
    [property: Display(Name = "Application")] string ApplicationName,
    [property: Display(Name = "Scopes")] OpenIdConnectScope[] Scopes
);