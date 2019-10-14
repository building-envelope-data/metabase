using System;
using Marten;
using System.Threading.Tasks;
using System.Collections.Generic;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Infrastructure.Aggregate {
public interface IAggregateRepository
{
    public Task<T> Store<T>(T aggregate, CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate;

    public Task<T> Load<T>(Guid id, int? version = null, CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate, new();

    public Task<IEnumerable<T>> LoadAll<T>(CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate, new();
}}
