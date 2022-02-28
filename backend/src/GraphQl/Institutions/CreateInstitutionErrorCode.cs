namespace Metabase.GraphQl.Institutions
{
    public enum CreateInstitutionErrorCode
    {
        UNKNOWN,
        NEITHER_OWNER_NOR_MANAGER,
        UNKNOWN_OWNERS,
        UNKNOWN_MANAGER,
        UNAUTHORIZED
    }
}