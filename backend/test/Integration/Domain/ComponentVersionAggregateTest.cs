using TestBase = Test.Integration.EventStore.TestBase;
using Xunit;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using ComponentVersionAggregate = Icon.Domain.ComponentVersionAggregate;
using ComponentVersion = Icon.Domain.ComponentVersion;
using System.Linq;

namespace Icon.Domain
{
    public sealed class ComponentVersionAggregateTest : TestBase
  {
    [Fact]
    public async Task Test()
    {
            // Arrange
            var component = await CreateComponent();
						var componentVersion = await CreateComponentVersion(component.Id);
            // Act
            var aggregate = await Session.Events.AggregateStreamAsync<ComponentVersionAggregate>(componentVersion.Id);
            // Assert
            aggregate.Should().BeEquivalentTo(componentVersion);
    }

		[Fact]
		public async Task InlineTest()
		{
            // Arrange
            var component = await CreateComponent();
						var componentVersion1 = await CreateComponentVersion(component.Id);
						var componentVersion2 = await CreateComponentVersion(component.Id);
						var componentVersion3 = await CreateComponentVersion(component.Id);
            // Act
						// TODO Shall we use a fresh session in other test too? It really makes a difference, because it does not use cached aggregates!
            var aggregates = CreateSession().Query<ComponentVersionAggregate>().ToList();
            // Assert
            aggregates.Should().BeEquivalentTo(componentVersion1, componentVersion2, componentVersion3);
		}

		private async Task<ComponentVersionAggregate> CreateComponentVersion(Guid componentId)
		{
            var command = new ComponentVersion.Create.Command(creatorId: Guid.NewGuid());
            command.ComponentId = componentId;
            var @event = new ComponentVersion.Create.ComponentVersionCreateEvent(Guid.NewGuid(), command);
            var componentVersion = ComponentVersionAggregate.Create(@event);
            var events = componentVersion.GetUncommittedEvents().ToArray();
            Session.Events.Append(componentVersion.Id, componentVersion.Version, events);
            await Session.SaveChangesAsync();
						return componentVersion;
		}

    private async Task<ComponentAggregate> CreateComponent()
    {
            var @event = new Component.Create.ComponentCreateEvent(Guid.NewGuid(), new Component.Create.Command(creatorId: Guid.NewGuid()));
            var component = ComponentAggregate.Create(@event);
            var events = component.GetUncommittedEvents().ToArray();
            Session.Events.Append(component.Id, component.Version, events);
            await Session.SaveChangesAsync();
            return component;
    }
  }
}
