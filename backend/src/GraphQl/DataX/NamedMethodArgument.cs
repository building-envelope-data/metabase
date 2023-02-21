using System.Text.Json;

namespace Metabase.GraphQl.DataX
{
    public sealed class NamedMethodArgument
    {
        public NamedMethodArgument(
        string name,
        JsonElement value
        )
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public JsonElement Value { get; }
    }
}