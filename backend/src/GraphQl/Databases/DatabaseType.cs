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
                "data",
                _ => _.GetDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureAllDataField<DataX.DataPropositionInput>(
                descriptor,
                "allData",
                _ => _.GetAllDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            );
            ConfigureHasDataField<DataX.DataPropositionInput>(
                descriptor,
                "hasData",
                _ => _.GetHasDataAsync(default!, default!, default!, default!, default!)
            );
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
            ConfigureHasDataField<DataX.OpticalDataPropositionInput>(
                descriptor,
                "hasOpticalData",
                _ => _.GetHasOpticalDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureDataField(
                descriptor,
                "hygrothermalData",
                _ => _.GetHygrothermalDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureAllDataField<DataX.HygrothermalDataPropositionInput>(
                descriptor,
                "allHygrothermalData",
                _ => _.GetAllHygrothermalDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            );
            ConfigureHasDataField<DataX.HygrothermalDataPropositionInput>(
                descriptor,
                "hasHygrothermalData",
                _ => _.GetHasHygrothermalDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureDataField(
                descriptor,
                "calorimetricData",
                _ => _.GetCalorimetricDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureAllDataField<DataX.CalorimetricDataPropositionInput>(
                descriptor,
                "allCalorimetricData",
                _ => _.GetAllCalorimetricDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            );
            ConfigureHasDataField<DataX.CalorimetricDataPropositionInput>(
                descriptor,
                "hasCalorimetricData",
                _ => _.GetHasCalorimetricDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureDataField(
                descriptor,
                "photovoltaicData",
                _ => _.GetPhotovoltaicDataAsync(default!, default!, default!, default!, default!)
            );
            ConfigureAllDataField<DataX.PhotovoltaicDataPropositionInput>(
                descriptor,
                "allPhotovoltaicData",
                _ => _.GetAllPhotovoltaicDataAsync(default!, default!, default!, default!, default!, default!, default!, default!, default!)
            );
            ConfigureHasDataField<DataX.PhotovoltaicDataPropositionInput>(
                descriptor,
                "hasPhotovoltaicData",
                _ => _.GetHasPhotovoltaicDataAsync(default!, default!, default!, default!, default!)
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
                .Argument("first", _ => _.Type<NonNegativeIntType>())
                .Argument("after", _ => _.Type<StringType>())
                .Argument("last", _ => _.Type<NonNegativeIntType>())
                .Argument("before", _ => _.Type<StringType>())
                .ResolveWith(resolverMethod);
        }

        private static void ConfigureHasDataField<TDataPropositionInput>(
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
                .ResolveWith(resolverMethod);
        }
    }
}