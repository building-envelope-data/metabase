using System;

namespace Metabase.GraphQl.Databases
{
    public record VerifyDatabaseInput(
          Guid DatabaseId
        );
}