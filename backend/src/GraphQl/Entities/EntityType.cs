using System;
using GreenDonut;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Scalars;

namespace Metabase.GraphQl.Entities;

public abstract class EntityType<TEntity, TEntityByIdDataLoader>
    : ObjectType<TEntity>
    where TEntity : IEntity
    where TEntityByIdDataLoader : IDataLoader<Guid, TEntity>
{
    protected override void Configure(
        IObjectTypeDescriptor<TEntity> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((context, id) =>
                    context
                        .DataLoader<TEntityByIdDataLoader>()
                        .LoadAsync(id, context.RequestAborted)
            );
        descriptor
            .Field(GraphQlConstants.UuidFieldName)
            .Type<NonNullType<UuidType>>()
            .Cost(0)
            .Resolve(context =>
                context.Parent<TEntity>().Id
            );
        // TODO Do we want to expose this, require it as input, and use it to discover concurrent writes?
        descriptor
            .Field(t => t.Version)
            .Type<NonNullType<NonNegativeIntType>>()
            .Name(GraphQlConstants.VersionFieldName)
            .Ignore();
    }
}