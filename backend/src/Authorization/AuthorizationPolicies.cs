namespace Metabase.Authorization;

public static class AuthorizationPolicies
{
    public const string ReadPolicy = "Read";
    public const string WritePolicy = "Write";
    public const string AdministratePolicy = "Administrate";
    public const string VerifyPolicy = "Verify";
    public const string ManageDatabasePolicy = "ManageDatabase";
    public const string ManageGnuPgPolicy = "ManageGnuPg";
    public const string ManageInstitutionRepresentativePolicy = "ManageInstitutionRepresentative";
    public const string ManageOpenIdConnectPolicy = "ManageOpenIdConnect";
    public const string ManageUserPolicy = "ManageUser";
}