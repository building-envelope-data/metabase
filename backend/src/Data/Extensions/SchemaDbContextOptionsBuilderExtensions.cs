using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

// Inspired by https://github.com/npgsql/efcore.pg/blob/main/src/EFCore.PG/Extensions/NpgsqlDbContextOptionsBuilderExtensions.cs
namespace Metabase.Data.Extensions;

public static class SchemaNameDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseSchemaName(
        this DbContextOptionsBuilder optionsBuilder,
        string schemaName
    )
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
            new SchemaNameOptionsExtension(schemaName)
        );
        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseSchemaName<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        string connectionString
    )
        where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)UseSchemaName(
            (DbContextOptionsBuilder)optionsBuilder, connectionString);
    }
}