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
            var @event = new Events.ComponentCreated(Guid.NewGuid(), new Commands.CreateComponent(creatorId: Guid.NewGuid()));
            var component = Aggregates.ComponentAggregate.Create(@event);
            var events = component.GetUncommittedEvents().ToArray();
            Session.Events.Append(component.Id, component.Version, events);
            await Session.SaveChangesAsync();
            // Act
            var aggregate = await Session.Events.AggregateStreamAsync<Aggregates.ComponentAggregate>(component.Id);
            // Assert
            aggregate.Should().BeEquivalentTo(component);
        }
    }
}