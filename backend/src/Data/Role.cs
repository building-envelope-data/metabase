using System;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Data;

public sealed class Role : IdentityRole<Guid>
{
    public const string Administrator = "Administrator";
    private const string Verifier = "Verifier";
    private const string Supporter = "Supporter";

    public static readonly ReadOnlyCollection<Enumerations.UserRole> AllEnum =
        Array.AsReadOnly(
        [
            Enumerations.UserRole.ADMINISTRATOR,
            Enumerations.UserRole.VERIFIER,
            Enumerations.UserRole.SUPPORTER
        ]);

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

    public static string EnumToName(Enumerations.UserRole role)
    {
        return role switch
        {
            Enumerations.UserRole.ADMINISTRATOR => Administrator,
            Enumerations.UserRole.VERIFIER => Verifier,
            Enumerations.UserRole.SUPPORTER => Supporter,
            _ => throw new ArgumentOutOfRangeException(nameof(role), $"Unknown role `{role}.`")
        };
    }

    public static Enumerations.UserRole EnumFromName(string name)
    {
        return name switch
        {
            Administrator => Enumerations.UserRole.ADMINISTRATOR,
            Verifier => Enumerations.UserRole.VERIFIER,
            Supporter => Enumerations.UserRole.SUPPORTER,
            _ => throw new ArgumentOutOfRangeException(nameof(name), $"Unknown name `{name}.`")
        };
    }
}