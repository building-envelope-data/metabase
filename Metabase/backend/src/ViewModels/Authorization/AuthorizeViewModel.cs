using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Metabase.ViewModels.Authorization
{
    public record AuthorizeViewModel
    {
        [Display(Name = "Application")]
        public string? ApplicationName { get; init; }

        [Display(Name = "Scope")]
        public string? Scope { get; init; }
    }
}