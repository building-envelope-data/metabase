using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NUnit.Framework;

namespace Metabase.Tests.Integration;

[TestFixture]
public sealed class MigrationTests
    : IntegrationTests
{
    // Inspired by https://www.meziantou.net/detect-missing-migrations-in-entity-framework-core.htm
    [Test]
    public Task EnsureMigrationsAreUpToDate()
    {
        return DoAsync(async context =>
        {
            // Get required services from the dbcontext
            var migrationModelDiffer = context.GetService<IMigrationsModelDiffer>();
            var migrationsAssembly = context.GetService<IMigrationsAssembly>();
            var modelRuntimeInitializer = context.GetService<IModelRuntimeInitializer>();
            var designTimeModel = context.GetService<IDesignTimeModel>();
            // Get current model
            var model = designTimeModel.Model;
            // Get the snapshot model and finalize it
            var snapshotModel = migrationsAssembly.ModelSnapshot?.Model;
            if (snapshotModel is IMutableModel mutableModel)
            {
                // Forces post-processing on the model such that it is ready for use by the runtime
                snapshotModel = mutableModel.FinalizeModel();
            }
            if (snapshotModel is not null)
            {
                // Validates and initializes the given model with runtime dependencies
                snapshotModel = modelRuntimeInitializer.Initialize(snapshotModel);
            }
            // Compute differences
            var modelDifferences = migrationModelDiffer.GetDifferences(
                    source: snapshotModel?.GetRelationalModel(),
                    target: model.GetRelationalModel());
            // The differences should be empty if the migrations are up-to-date
            modelDifferences.Should().BeEquivalentTo(Enumerable.Empty<MigrationOperation>()); // .BeEmpty();
        });
    }
}