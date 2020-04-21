using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Icon.Models
{
    public class User : IdentityUser<Guid>, IModel
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }

        /* public ICollection<Product> Products { get; set; } */
        /* public ICollection<MeasurementMethod> MeasurementMethods { get; set; } */
    }
}