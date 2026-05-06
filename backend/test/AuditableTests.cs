using NodaTime;
using NodaTime.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Metabase.Data;
using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Metabase.Tests;

[TestFixture]
public sealed class AuditableTests
{
    private DbContextOptions<ApplicationDbContext> _options = default!;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Test]
    [SuppressMessage("Naming", "CA1707")]
    public async Task SaveChanges_SetsAndUpdatesTimestamps_UsingFakeClock()
    {
        // Arrange
        var startInstant = Instant.FromUtc(2024, 1, 1, 10, 0);
        var fakeClock = new FakeClock(startInstant);
        using var context = new ApplicationDbContext(_options, fakeClock);
        var entity = new Method("Name", "Description", null, null, null, [], [], []);
        // Act
        context.Add(entity);
        await context.SaveChangesAsync();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.CreatedAt, Is.EqualTo(startInstant.WithOffset(Offset.Zero)));
            Assert.That(entity.UpdatedAt, Is.EqualTo(startInstant.WithOffset(Offset.Zero)));
        });
        // Act
        var duration = Duration.FromHours(1);
        fakeClock.Advance(duration);
        var updatedInstant = startInstant.Plus(duration);
        entity.Update("Name", "Description", null, null, null, [], [], []);
        await context.SaveChangesAsync();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entity.CreatedAt, Is.EqualTo(startInstant.WithOffset(Offset.Zero)), "CreatedAt should not change on update.");
            Assert.That(entity.UpdatedAt, Is.EqualTo(updatedInstant.WithOffset(Offset.Zero)), "UpdatedAt should reflect the new fake time.");
        });
    }

    // [Test]
    // public async Task Remove_PerformsSoftDelete_AndSetsDeletedAt()
    // {
    //     var deleteTime = Instant.FromUtc(2024, 1, 1, 15, 0);
    //     var fakeClock = new FakeClock(deleteTime);
    //     using var context = new ApplicationDbContext(_options, fakeClock);
    //     var entity = new YourModel { Name = "To Be Deleted" };
    //     context.Add(entity);
    //     await context.SaveChangesAsync();
    //     // Act
    //     context.Remove(entity);
    //     await context.SaveChangesAsync();
    //     // Assert
    //     Assert.That(entity.DeletedAt, Is.EqualTo(deleteTime));
    //     // Ensure it's hidden from normal queries
    //     var count = await context.YourModels.CountAsync();
    //     Assert.That(count, Is.Zero);
    // }
}