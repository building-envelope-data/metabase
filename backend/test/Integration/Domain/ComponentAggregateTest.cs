using TestBase = Test.Integration.EventStore.TestBase;
using Xunit;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using ComponentAggregate = Icon.Domain.ComponentAggregate;
using Component = Icon.Domain.Component;
using System.Linq;

namespace Icon.Domain
{
    public sealed class ComponentAggregateTest : TestBase
    {
        [Fact]
        public async Task Test()
        {
            // Arrange
            var @event = new Component.Create.ComponentCreateEvent(Guid.NewGuid(), new Component.Create.Command(creatorId: Guid.NewGuid()));
            var component = ComponentAggregate.Create(@event);
            var events = component.GetUncommittedEvents().ToArray();
            Session.Events.Append(component.Id, component.Version, events);
            await Session.SaveChangesAsync();
            // Act
            var aggregate = await Session.Events.AggregateStreamAsync<ComponentAggregate>(component.Id);
            // Assert
            aggregate.Should().BeEquivalentTo(component);
        }
    }
}