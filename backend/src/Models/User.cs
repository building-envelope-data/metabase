using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable
namespace Icon.Models
{
    public class User : IdentityUser<Guid>
    {
        /* public ICollection<Product> Products { get; set; } */
        /* public ICollection<MeasurementMethod> MeasurementMethods { get; set; } */
    }
}