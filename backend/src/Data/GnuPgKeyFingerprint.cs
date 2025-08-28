using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Index(nameof(Fingerprint), IsUnique = true)]
public sealed class GnuPgKeyFingerprint(
    string fingerprint
    )
        : Entity
{
    [Required][MinLength(1)] public string Fingerprint { get; private set; } = fingerprint;

    [Required] public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? AllowedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public Guid UserId { get; set; }
    [InverseProperty(nameof(User.GnuPgKeyFingerprints))]
    public User? User { get; set; }

    public Guid InstitutionId { get; set; }
    [InverseProperty(nameof(Institution.GnuPgKeyFingerprints))]
    public Institution? Institution { get; set; }

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }

    public bool IsRevoked => RevokedAt is not null;

    public void Allow()
    {
        AllowedAt = DateTime.UtcNow;
    }

    public bool IsAllowed => AllowedAt is not null;
}