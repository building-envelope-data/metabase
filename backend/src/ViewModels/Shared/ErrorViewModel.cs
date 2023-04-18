using System.ComponentModel.DataAnnotations;

namespace Metabase.ViewModels.Shared
{
    public sealed class ErrorViewModel
    {
        [Display(Name = "Error")]
        public string? Error { get; set; }

        [Display(Name = "Description")]
        public string? ErrorDescription { get; set; }
    }
}