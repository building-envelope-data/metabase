using System;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectScope
    : OpenIddictEntityFrameworkCoreScope<Guid>,
      IEntity,
      IAuditable
{
    private const string ScopeSeparator = ":";

    private const string ApiScopePrefix = "api";
    // Allow also non-public GraphQL queries
    public const string ReadApiScope = ApiScopePrefix + ScopeSeparator + "read";
    // Allow write-access, that is, GraphQL mutations
    public const string WriteApiScope = ApiScopePrefix + ScopeSeparator + "write";
    // Allow elevated privileges for users with role "Administrator"
    public const string AdministrateApiScope = ApiScopePrefix + ScopeSeparator + "administrate";
    // Allow elevated privileges for users with role "Verifier"
    public const string VerifyApiScope = ApiScopePrefix + ScopeSeparator + "verify";
    // Allow elevated privileges for users with role "Supporter"
    public const string SupportApiScope = ApiScopePrefix + ScopeSeparator + "support";
    public const string ManageDatabaseApiScope = ApiScopePrefix + ScopeSeparator + "database" + ScopeSeparator + "manage";
    public const string ManageGnuPgApiScope = ApiScopePrefix + ScopeSeparator + "gnu_pg" + ScopeSeparator + "manage";
    public const string ManageInstitutionRepresentativeApiScope = ApiScopePrefix + ScopeSeparator + "institution_representative" + ScopeSeparator + "manage";
    public const string ManageOpenIdConnectApiScope = ApiScopePrefix + ScopeSeparator + "open_id_connect" + ScopeSeparator + "manage";
    // Allow user management (essentially the usage of user mutations)
    public const string ManageUserApiScope = ApiScopePrefix + ScopeSeparator + "user" + ScopeSeparator + "manage";

    public static readonly string[] Scopes =
    [
        OpenIddictConstants.Scopes.Address,
        OpenIddictConstants.Scopes.Email,
        OpenIddictConstants.Scopes.Phone,
        OpenIddictConstants.Scopes.Profile,
        OpenIddictConstants.Scopes.Roles,
        ReadApiScope,
        WriteApiScope,
        AdministrateApiScope,
        VerifyApiScope,
        SupportApiScope,
        ManageDatabaseApiScope,
        ManageGnuPgApiScope,
        ManageInstitutionRepresentativeApiScope,
        ManageOpenIdConnectApiScope,
        ManageUserApiScope,
    ];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // Configured via `IsRowVersion` in `ApplicationDbContext` instead of the annotation
    // [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}