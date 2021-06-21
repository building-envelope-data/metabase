using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;
using System;

namespace Metabase.Data
{
    public sealed class Role : IdentityRole<Guid>
    {
        public static readonly ReadOnlyCollection<string> All =
            Array.AsReadOnly(new[] {
                Administrator
            });

        public const string Administrator = "Administrator";

        // public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();

        public Role()
        {
        }

        public Role(string name)
            : base(name)
        {
        }
    }
}