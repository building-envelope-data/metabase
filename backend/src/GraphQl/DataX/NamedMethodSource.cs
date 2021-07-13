namespace Metabase.GraphQl.DataX
{
    public sealed class NamedMethodSource
    {
      public NamedMethodSource(
      string name,
      CrossDatabaseDataReference value
      )
      {
      Name = name;
      Value = value;
      }

      public string Name { get; }
      public CrossDatabaseDataReference Value { get; }
    }
}