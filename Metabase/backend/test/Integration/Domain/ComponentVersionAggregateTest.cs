using TestBase = Test.Integration.EventStore.TestBase;

namespace Metabase.Domain
{
    public sealed class ComponentVersionAggregateTest : TestBase
    {
        /* [Fact] */
        /* public async Task Test() */
        /* { */
        /*     // Arrange */
        /*     var component = await CreateComponent(); */
        /*     var componentVersion = await CreateComponentVersion(component.Id); */
        /*     // Act */
        /*     var aggregate = await Session.Events.AggregateStreamAsync<Aggregates.ComponentVersionAggregate>(componentVersion.Id); */
        /*     // Assert */
        /*     aggregate.Should().BeEquivalentTo(componentVersion); */
        /* } */

        /* [Fact] */
        /* public async Task InlineTest() */
        /* { */
        /*     // Arrange */
        /*     var component = await CreateComponent(); */
        /*     var componentVersion1 = await CreateComponentVersion(component.Id); */
        /*     var componentVersion2 = await CreateComponentVersion(component.Id); */
        /*     var componentVersion3 = await CreateComponentVersion(component.Id); */
        /*     // Act */
        /*     // TODO Shall we use a fresh session in other test too? It really makes a difference, because it does not use cached aggregates! */
        /*     var aggregates = CreateSession().Query<Aggregates.ComponentVersionAggregate>().ToList(); */
        /*     // Assert */
        /*     aggregates.Should().BeEquivalentTo(componentVersion1, componentVersion2, componentVersion3); */
        /* } */

        /* private async Task<Aggregates.ComponentVersionAggregate> CreateComponentVersion(Guid componentId) */
        /* { */
        /*     var command = new Commands.CreateComponentVersion(componentId, creatorId: Guid.NewGuid()); */
        /*     var @event = new Events.ComponentVersionCreated(Guid.NewGuid(), command); */
        /*     var componentVersion = Aggregates.ComponentVersionAggregate.Create(@event); */
        /*     var events = componentVersion.GetUncommittedEvents().ToArray(); */
        /*     Session.Events.Append(componentVersion.Id, componentVersion.Version, events); */
        /*     await Session.SaveChangesAsync(); */
        /*     return componentVersion; */
        /* } */

        /* private async Task<Aggregates.ComponentAggregate> CreateComponent() */
        /* { */
        /*     var @event = new Events.ComponentCreated(Guid.NewGuid(), new Commands.CreateComponent(creatorId: Guid.NewGuid())); */
        /*     var component = Aggregates.ComponentAggregate.Create(@event); */
        /*     var events = component.GetUncommittedEvents().ToArray(); */
        /*     Session.Events.Append(component.Id, component.Version, events); */
        /*     await Session.SaveChangesAsync(); */
        /*     return component; */
        /* } */
    }
}