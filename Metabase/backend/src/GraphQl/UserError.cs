using System.Collections.Generic;

// See section `Stage 6a: ...` in https://productionreadygraphql.com/2020-08-01-guide-to-graphql-errors
namespace Metabase.GraphQl
{
  public interface UserError
  {
    public string Message { get; }
    public IReadOnlyList<string> Path { get; }
  }
}
