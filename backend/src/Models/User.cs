using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Icon.Models
{
    public class User : IdentityUser<Guid>// , IModel
    {
        /* [NotMapped] */
        /* public ValueObjects.Id Id { get; } */

        /* [NotMapped] */
        /* public ValueObjects.Timestamp Timestamp { get; } */
    }
}