using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;
using System;

namespace Metabase.Data
{
    public sealed class Role : IdentityRole<Guid>
    {
        private const string Administrator = "Administrator";
        private const string Verifier = "Verifier";

        public static readonly ReadOnlyCollection<Enumerations.UserRole> AllEnum =
            Array.AsReadOnly(new[]
            {
                Enumerations.UserRole.ADMINISTRATOR,
                Enumerations.UserRole.VERIFIER
            });

        public static string EnumToName(Enumerations.UserRole role)
        {
            return role switch
            {
                Enumerations.UserRole.ADMINISTRATOR => Administrator,
                Enumerations.UserRole.VERIFIER => Verifier,
                _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role `{role}.`")
            };
        }

        public static Enumerations.UserRole EnumFromName(string name)
        {
            return name switch
            {
                Administrator => Enumerations.UserRole.ADMINISTRATOR,
                Verifier => Enumerations.UserRole.VERIFIER,
                _ => throw new ArgumentOutOfRangeException(nameof(name), $"Unknown name `{name}.`")
            };
        }

        // public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();

        public Role()
        {
        }

        public Role(string name)
            : base(name)
        {
        }

        public Role(Enumerations.UserRole role)
            : base(EnumToName(role))
        {
        }
    }
}