using System.ComponentModel.DataAnnotations;

namespace Metabase.ViewModels.Authorization;

public sealed record AuthorizeViewModel
{
    [Display(Name = "Application")] public string? ApplicationName { get; init; }

    [Display(Name = "Scope")] public string? Scope { get; init; }
}