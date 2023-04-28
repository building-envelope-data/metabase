using System;

namespace Metabase.GraphQl.Databases
{
    public sealed record VerifyDatabaseInput(
          Guid DatabaseId
        );
}