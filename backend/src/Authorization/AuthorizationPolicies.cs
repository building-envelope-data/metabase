namespace Metabase.Authorization;

public static class AuthorizationPolicies
{
    public const string AuthenticatedPolicy = "Authenticated";
    public const string ReadScopePolicy = "ReadScope";
    public const string WriteScopePolicy = "WriteScope";
    public const string AdministrateScopePolicy = "AdministrateScope";
    public const string VerifyScopePolicy = "VerifyScope";
    public const string ManageDatabaseScopePolicy = "ManageDatabaseScope";
    public const string ManageGnuPgScopePolicy = "ManageGnuPgScope";
    public const string ManageInstitutionRepresentativeScopePolicy = "ManageInstitutionRepresentativeScope";
    public const string ManageOpenIdConnectScopePolicy = "ManageOpenIdConnectScope";
    public const string ManageUserScopePolicy = "ManageUserScope";
}