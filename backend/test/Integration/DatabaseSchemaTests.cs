using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Npgsql;
using NUnit.Framework;

namespace Metabase.Tests.Integration;

[TestFixture]
public sealed class DatabaseSchemaTests
    : IntegrationTests
{
    /// <summary>
    /// Compare the database schema wanted by `ApplicationDbContext` to the
    /// database schema constructed by `./database.mk create migrate`. The
    /// former is the database schema that matches the domain model and the
    /// latter is the actual database schema being used. They can get
    /// out-of-sync if manual changes to `migrate.sql` introduce
    /// inconsistencies or if changes to the domain model were made but no
    /// `make migration NAME=...` run yet.
    ///
    /// Do not use
    /// [`EfCore.SchemaCompare`](https://github.com/JonPSmith/EfCore.SchemaCompare)
    /// to reverse-engineer the domain model from the PostgreSQL database schema
    /// and compare it with the actual one because of its
    /// [limitations](https://github.com/JonPSmith/EfCore.SchemaCompare#List-of-limitations).
    /// An alternative to
    /// [`pg-schema-diff`](https://github.com/stripe/pg-schema-diff)
    /// it
    /// [`liquibase diff`](https://docs.liquibase.com/community/reference-guide-5-0-3/database-inspection-change-tracking-and-utility-commands/diff)
    /// which can be used as follows
    /// ```
    /// liquibase diff \
    ///    --url="jdbc:postgresql://database:5432/xbase" --username="postgres" --password="postgres" --default-schema-name="metabase" \
    ///    --reference-url="jdbc:postgresql://database:5432/test" --reference-username="postgres" --reference-password="postgres" --reference-default-schema-name="metabase"
    /// ```
    /// and installed as explained on
    /// [Install Liquibase on Linux with Debian or Ubuntu](https://docs.liquibase.com/community/get-started-5-0-3/install-liquibase-on-linux-with-debian-or-ubuntu)
    /// </summary>
    [Test]
    public async Task EnsureDomainModelMatchesDatabaseSchema()
    {
        // Arrange
        var cancellationToken = TestContext.CurrentContext.CancellationToken;
        var migrateSqlBasedDatabaseName = Guid.NewGuid().ToString().Replace("-", "");
        await ExecuteSqlScriptAsync(
            AppSettings.Database.ConnectionString(),
            $"""CREATE DATABASE "{migrateSqlBasedDatabaseName}";""",
            cancellationToken
        );
        await ExecuteSqlScriptAsync(
            AppSettings.Database.ConnectionString(migrateSqlBasedDatabaseName),
            // the current directory, that is, `./`, is `/home/me/app/test/bin/Debug/net*.0/`
            await ReadSqlScriptAsync(Path.Combine("..", "..", "..", "..", "src", "Migrations", "migrate.sql"), cancellationToken),
            cancellationToken
        );
        // Act
        var differenceResponse = await ExecuteCommandAsync(
            $"""
            pg-schema-diff plan \
                --from-dsn "postgres://{AppSettings.Database.UserName}:{AppSettings.Database.Password}@database/{AppSettings.Database.Name}?search_path={AppSettings.Database.SchemaName}" \
                --to-dsn "postgres://{AppSettings.Database.UserName}:{AppSettings.Database.Password}@database/{migrateSqlBasedDatabaseName}?search_path={AppSettings.Database.SchemaName}"
            """,
            null,
            cancellationToken
        );
        // Assert
        differenceResponse.ExitCode.Should().Be(
            0,
            because: differenceResponse.Output + "\n" + differenceResponse.Diagnostics + "\nIf the test `EnsureMigrationsAreUpToDate` also fails, then run `make migration NAME='...' in the shell `./docker.mk shell SERVICE=backend`. Otherwise, most probably, manual changes to ./migrate.sql introduced inconsistencies: Resolve them by creating an empty migration with `make migration NAME='...' in the shell `./docker.mk shell SERVICE=backend` and adapting the 4 files in ./backend/src/Migrations as needed."
        );
    }

    private static Task<string> ReadSqlScriptAsync(
        string scriptPath,
        CancellationToken cancellationToken
    )
    {
        var absoluteScriptPath = Path.GetFullPath(scriptPath);
        if (!File.Exists(absoluteScriptPath))
        {
            throw new FileNotFoundException($"The script file was not found at: {absoluteScriptPath}");
        }
        return File.ReadAllTextAsync(absoluteScriptPath, cancellationToken);
    }

    private static async Task ExecuteSqlScriptAsync(
        string connectionString,
        string script,
        CancellationToken cancellationToken
    )
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        // If necessary, the transactions can be started and committed within the script.
        // await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            await using var command = new NpgsqlCommand(script, connection); // , transaction
            await command.ExecuteNonQueryAsync(cancellationToken);
            // await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            // await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static async Task<(int ExitCode, string Output, string Diagnostics)> ExecuteCommandAsync(
        string command,
        string? input,
        CancellationToken cancellationToken
    )
    {
        var process = Process.Start(
            new ProcessStartInfo()
            {
                FileName = "bash",
                ArgumentList = {
                    "-o", "errexit",
                    "-o", "errtrace",
                    "-o", "nounset",
                    "-o", "pipefail",
                    "-c", command
                },
                ErrorDialog = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }
        ) ?? throw new InvalidOperationException($"The process for command '{command}' with the input '{input}' failed to start.");
        if (input is not null)
        {
            await process.StandardInput.WriteLineAsync(input.AsMemory(), cancellationToken);
            await process.StandardInput.FlushAsync(cancellationToken);
        }
        process.StandardInput.Close();
        var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        var diagnostics = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        return (process.ExitCode, output, diagnostics);
    }
}