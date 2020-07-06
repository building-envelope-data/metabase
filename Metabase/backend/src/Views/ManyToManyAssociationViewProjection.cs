/* using Type = System.Type; */
/* using Guid = System.Guid; */
/* using System; // Func<,> */
/* using System.Collections.Generic; */
/* using System.Threading.Tasks; */
/* using CancellationToken = System.Threading.CancellationToken; */
/* using Marten; */
/* using Marten.Events.Projections.Async; */
/* using Marten.Events.Projections; */
/* using Marten.Storage; */

/* namespace Metabase.Views */
/* { */
/*     public sealed class ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent> */
/*       : IProjection */
/*       where TAssociationAddedEvent : Events.IAssociationAddedEvent */
/*       where TAssociationRemovedEvent : Events.IAssociationRemovedEvent */
/*     { */
/*         public static ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent> ForwardAssociates() */
/*         { */
/*             return new ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent>( */
/*                 addedEvent => (addedEvent.ParentId, addedEvent.AssociateId), */
/*                 removedEvent => (removedEvent.ParentId, removedEvent.AssociateId) */
/*                 ); */
/*         } */

/*         public static ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent> ForwardAssociations() */
/*         { */
/*             return new ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent>( */
/*                 addedEvent => (addedEvent.ParentId, addedEvent.AggregateId), */
/*                 removedEvent => (removedEvent.ParentId, removedEvent.AggregateId) */
/*                 ); */
/*         } */

/*         public static ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent> BackwardAssociates() */
/*         { */
/*             return new ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent>( */
/*                 addedEvent => (addedEvent.AssociateId, addedEvent.ParentId), */
/*                 removedEvent => (removedEvent.AssociateId, removedEvent.ParentId) */
/*                 ); */
/*         } */

/*         public static ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent> BackwardAssociations() */
/*         { */
/*             return new ManyToManyAssociationViewProjection<TAssociationAddedEvent, TAssociationRemovedEvent>( */
/*                 addedEvent => (addedEvent.AssociateId, addedEvent.AggregateId), */
/*                 removedEvent => (removedEvent.AssociateId, removedEvent.AggregateId) */
/*                 ); */
/*         } */

/*         private readonly Func<TAssociationAddedEvent, (Guid, Guid)> _addedIdsSelector; */
/*         private readonly Func<TAssociationRemovedEvent, (Guid, Guid)> _removedIdsSelector; */
/*         private readonly IDictionary<Guid, ISet<Guid>> _idToAssociationIds; */

/*         public Type[] Consumes { get; } */
/*         public AsyncOptions AsyncOptions { get; } */

/*         public ManyToManyAssociationViewProjection( */
/*             Func<TAssociationAddedEvent, (Guid, Guid)> addedIdsSelector, */
/*             Func<TAssociationRemovedEvent, (Guid, Guid)> removedIdsSelector */
/*             ) */
/*         { */
/*             _addedIdsSelector = addedIdsSelector; */
/*             _removedIdsSelector = removedIdsSelector; */
/*             _idToAssociationIds = new Dictionary<Guid, ISet<Guid>>(); */
/*             Consumes = new Type[] { */
/*               typeof(TAssociationAddedEvent), */
/*               typeof(TAssociationRemovedEvent) */
/*             }; */
/*             AsyncOptions = new AsyncOptions(); */
/*         } */

/*         public void Apply(IDocumentSession session, EventPage page) */
/*         { */
/*             /1* foreach (var stream in page.Streams) *1/ */
/*             /1* { *1/ */
/*             /1*     foreach (var @event in stream.Events.OfType<Event<TEvent>>()) *1/ */
/*             /1*     { *1/ */
/*             /1*     } *1/ */
/*             /1* } *1/ */
/*             foreach (var @event in page.Events) */
/*             { */
/*                 if (@event.Data is TAssociationAddedEvent addedData) */
/*                 { */
/*                     var (viewId, otherId) = _addedIdsSelector(addedData); */
/*                     if (!_idToAssociationIds.ContainsKey(viewId)) */
/*                     { */
/*                         _idToAssociationIds.Add(viewId, new HashSet<Guid>()); */
/*                     } */
/*                     _idToAssociationIds[viewId].Add(otherId); */
/*                 } */
/*                 else if (@event.Data is TAssociationRemovedEvent removedData) */
/*                 { */
/*                     var (viewId, otherId) = _removedIdsSelector(removedData); */
/*                     _idToAssociationIds[viewId].Remove(otherId); */
/*                 } */
/*                 else */
/*                 { */
/*                     throw new Exception($"The event {@event} is neither of type {typeof(TAssociationAddedEvent)} nor of type {typeof(TAssociationRemovedEvent)}"); */
/*                 } */
/*             } */
/*         } */

/*         public Task ApplyAsync(IDocumentSession session, EventPage page, CancellationToken token) */
/*         { */
/*             Apply(session, page); */
/*             return Task.CompletedTask; */
/*         } */

/*         public void EnsureStorageExists(ITenant tenant) */
/*         { */
/*             // TODO What should this method do? */
/*             // https://github.com/JasperFx/marten/blob/ccf3572e4237e085995237016bf65cb62f8e7dde/src/Marten/Events/Projections/IDocumentProjection.cs#L20 */
/*             // tenant.EnsureStorageExists(Produces); */
/*         } */
/*     } */
/* } */