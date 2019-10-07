using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Icon.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
      public ICollection<Product> Products { get; set; }
    }
}