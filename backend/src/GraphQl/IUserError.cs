using System.Collections.Generic;
using HotChocolate.Types;

// See section `Stage 6a: ...` in https://productionreadygraphql.com/2020-08-01-guide-to-graphql-errors
namespace Metabase.GraphQl;

[InterfaceType("UserError")]
public interface IUserError
{
    public string Message { get; }
    public IReadOnlyList<string> Path { get; }
}