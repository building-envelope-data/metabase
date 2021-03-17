using HotChocolate.Types;

namespace Metabase.GraphQl.Databases
{
    public sealed class DatabaseType
      : EntityType<Data.Database, DatabaseByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Database> descriptor
            )
        {
            base.Configure(descriptor);
            descriptor
                .Field(t => t.Operator)
                .Type<NonNullType<ObjectType<DatabaseOperatorEdge>>>()
                .Resolve(context =>
                    new DatabaseOperatorEdge(
                        context.Parent<Data.Database>()
                        )
                    );
            descriptor
                .Field(t => t.OperatorId).Ignore();
        }
    }
}