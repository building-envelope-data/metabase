namespace Metabase.GraphQl.Databases
{
    public record CreateDatabaseInput(
          string Name,
          string Description,
          string Locator
        );
}
