using System;
using System.Linq.Expressions;
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
            ConfigureDataField(
                descriptor,
                "opticalData",
                _ => _.GetOpticalDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureAllDataField<DataX.OpticalDataPropositionInput>(
                descriptor,
                "allOpticalData",
                _ => _.GetAllOpticalDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            );
        }

        private static void ConfigureDataField(
            IObjectTypeDescriptor<Data.Database> descriptor,
            string fieldName,
            Expression<Func<DatabaseResolvers, object?>> resolverMethod
        )
        {
            descriptor
                .Field(fieldName)
                .Argument("id", _ => _.Type<NonNullType<UuidType>>())
                .Argument("timestamp", _ => _.Type<DateTimeType>())
                .Argument("locale", _ => _.Type<StringType>())
                .ResolveWith(resolverMethod);
        }

        private static void ConfigureAllDataField<TDataPropositionInput>(
            IObjectTypeDescriptor<Data.Database> descriptor,
            string fieldName,
            Expression<Func<DatabaseResolvers, object?>> resolverMethod
        )
        {
            descriptor
                .Field(fieldName)
                .Argument("where", _ => _.Type<NonNullType<InputObjectType<TDataPropositionInput>>>())
                .Argument("timestamp", _ => _.Type<DateTimeType>())
                .Argument("locale", _ => _.Type<StringType>())
                .Argument("first", _ => _.Type</*TODO Unsigned*/IntType>())
                .Argument("after", _ => _.Type<StringType>())
                .Argument("last", _ => _.Type</*TODO Unsigned*/IntType>())
                .Argument("before", _ => _.Type<StringType>())
                .ResolveWith(resolverMethod);
        }
    }
}