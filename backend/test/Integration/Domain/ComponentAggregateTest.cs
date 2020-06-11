using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Aggregates = Icon.Aggregates;
using Commands = Icon.Commands;
using Events = Icon.Events;
using TestBase = Test.Integration.EventStore.TestBase;

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
                information: new Events.ComponentInformationEventData(
                  name: "My Name",
                  abbreviation: null,
                  description: "My Description",
                  availableFrom: null,
                  availableUntil: null,
                  categories: new List<Events.ComponentCategoryEventData>()
                  ),
                creatorId: Guid.NewGuid()
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