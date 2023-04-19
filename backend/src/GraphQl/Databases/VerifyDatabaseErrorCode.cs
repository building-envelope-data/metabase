namespace Metabase.GraphQl.Databases
{
    public enum VerifyDatabaseErrorCode
    {
        UNKNOWN,
        UNAUTHORIZED,
        UNKNOWN_DATABASE,
        WRONG_VERIFICATION_CODE,
        REQUEST_FAILED,
        DESERIALIZATION_FAILED
    }
}