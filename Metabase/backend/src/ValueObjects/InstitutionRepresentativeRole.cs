namespace Metabase.ValueObjects
{
    // What enums are is explained in https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum
    // How they can be used with PostgreSQL is explained in https://www.npgsql.org/doc/types/enums_and_composites.html
    // A more powerful way to model enums is detailed in https://stackoverflow.com/a/1343517
    // A way to turn enum values into strings is detailed in https://stackoverflow.com/a/630900
    public enum InstitutionRepresentativeRole
    {
        OWNER,
        MAINTAINER,
        ASSISTANT
    }
}