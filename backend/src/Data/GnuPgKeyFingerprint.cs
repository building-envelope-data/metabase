using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Index(nameof(Fingerprint), IsUnique = true)]
public sealed partial class GnuPgKeyFingerprint(
    string fingerprint
    )
        : Entity
{
    [GeneratedRegex("[^A-F0-9]")]
    private static partial Regex HexadecimalRegex();

    public static string Normalize(string dirtyFingerprint)
    {
        return HexadecimalRegex().Replace(
            dirtyFingerprint.ToUpperInvariant(),
            string.Empty
        );
    }

    [Required][MinLength(1)] public string Fingerprint { get; private set; } = Normalize(fingerprint);

    [Required] public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? AllowedAt { get; private set; }
    public DateTime? ForbiddenAt { get; private set; }

    public Guid UserId { get; set; }
    [InverseProperty(nameof(User.GnuPgKeyFingerprints))]
    public User? User { get; set; }

    public Guid InstitutionId { get; set; }
    [InverseProperty(nameof(Institution.GnuPgKeyFingerprints))]
    public Institution? Institution { get; set; }

    public void Allow()
    {
        AllowedAt ??= DateTime.UtcNow;
    }

    public bool IsAllowed => AllowedAt is not null;

    public void Forbid()
    {
        // If this fingerprint has not been allowed for approval yet before it
        // shall be forbidden now, we set `AllowedAt` and `ForbiddenAt` to
        // the present moment making its total validity range the half closed
        // interval `[AllowedAt, ForbiddenAt)` empty. This makes sure that
        // whenever `ForbiddenAt` is set, `AllowedAt` is also set.
        var now = DateTime.UtcNow;
        AllowedAt ??= now;
        ForbiddenAt ??= now;
    }

    public bool IsForbidden => ForbiddenAt is not null;
}