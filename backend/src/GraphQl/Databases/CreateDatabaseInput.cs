using System;

namespace Metabase.GraphQl.Databases;

public sealed record CreateDatabaseInput(
    string Name,
    string Description,
    Uri Locator,
    Guid OperatorId
);