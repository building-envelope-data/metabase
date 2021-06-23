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
            descriptor
                .Field("allOpticalData")
                .Argument("where", _ => _.Type<NonNullType<InputObjectType<DataX.OpticalDataPropositionInput>>>())
                .Argument("timestamp", _ => _.Type<DateTimeType>())
                .Argument("locale", _ => _.Type<StringType>())
                .Argument("first", _ => _.Type</*TODO Unsigned*/IntType>())
                .Argument("after", _ => _.Type<StringType>())
                .Argument("last", _ => _.Type</*TODO Unsigned*/IntType>())
                .Argument("before", _ => _.Type<StringType>())
                .ResolveWith<DatabaseResolvers>(_ =>
                    _.GetAllOpticalDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
                );
        }
    }
}