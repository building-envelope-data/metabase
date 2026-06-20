using System;
using System.Globalization;
using EfSchemaCompare;
using FluentAssertions;
using Metabase.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Serilog;

namespace Metabase.Tests.Integration;

[TestFixture]
public sealed class DatabaseSchemaTests
    : IDisposable
{
    private sealed class CustomWebApplicationFactory
        : WebApplicationFactory<Metabase.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Metabase.Program.DevelopmentEnvironment);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration) // appsettings.test.json
                    .WriteTo.NUnitOutput(formatProvider: CultureInfo.InvariantCulture);
            });
            return base.CreateHost(builder);
        }
    }

    private bool _disposed;
    private CustomWebApplicationFactory Factory { get; }

    public DatabaseSchemaTests()
    {
        Factory = new CustomWebApplicationFactory();
    }

    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // https://docs.microsoft.com/en-us/dotnet/standard/managed-code
    // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
    ~DatabaseSchemaTests()
    {
        Dispose(false);
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Factory.Dispose();
            }
            _disposed = true;
        }
    }

    // For a list of limitations see https://github.com/JonPSmith/EfCore.SchemaCompare#List-of-limitations
    [Test]
    public void EnsureDomainModelMatchesDatabaseSchema()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();
        var databaseContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var connectionString = databaseContext.Database.GetConnectionString() + $";Password={appSettings.Database.Password}";
        var comparer = new CompareEfSql();
        // Act
        // Compare C# domain model with PostgreSQL database schema, that is,
        // compare EntityFramework Core model of the database with the database
        // that the database context's connection points to.
        var hasErrors = comparer.CompareEfWithDb(connectionString, databaseContext);
        // Assert
        hasErrors.Should().BeFalse(comparer.GetAllErrors);
    }
}