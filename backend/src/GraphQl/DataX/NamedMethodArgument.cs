namespace Metabase.GraphQl.DataX
{
    public sealed class NamedMethodArgument {
      public NamedMethodArgument(
      string name,
      object value
      )
      {
      Name = name;
      Value = value;
      }

      public string Name { get; }
      public object Value { get; }
    }
}