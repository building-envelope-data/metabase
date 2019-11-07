using TestBase = Test.Integration.EventStore.TestBase;
using Xunit;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using System.Linq;
using Aggregates = Icon.Aggregates;
using Commands = Icon.Commands;
using Events = Icon.Events;

namespace Icon.Domain
{
    public sealed class ComponentAggregateTest : TestBase
    {
        [Fact]
        public async Task Test()
        {
            // Arrange
            var id = Guid.NewGuid();
            var @event = new Events.ComponentCreated(
                id,
                new Commands.CreateComponent(creatorId: Guid.NewGuid())
                );
            Session.Events.Append(id, 1, @event);
            await Session.SaveChangesAsync();
            // Act
            var aggregate = await Session.Events.AggregateStreamAsync<Aggregates.ComponentAggregate>(id);
            // Assert
            aggregate.Id.Should().Be(id);
            aggregate.Version.Should().Be(1);
        }
    }
}