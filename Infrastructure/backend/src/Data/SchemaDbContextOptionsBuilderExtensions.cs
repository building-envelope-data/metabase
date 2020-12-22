using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

// Inspired by https://github.com/npgsql/efcore.pg/blob/main/src/EFCore.PG/Extensions/NpgsqlDbContextOptionsBuilderExtensions.cs
namespace Infrastructure.Data
{
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
      => (DbContextOptionsBuilder<TContext>)UseSchemaName(
          (DbContextOptionsBuilder)optionsBuilder, connectionString);
  }
}
