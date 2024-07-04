using System;

namespace Metabase.GraphQl.Databases;

public sealed record UpdateDatabaseInput(
    Guid DatabaseId,
    string Name,
    string Description,
    Uri Locator
);