using System;
using System.Linq.Expressions;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.DataX;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseType
    : EntityType<Database, DatabaseByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.Operator)
            .Type<NonNullType<ObjectType<DatabaseOperatorEdge>>>()
            .Resolve(context =>
                new DatabaseOperatorEdge(
                    context.Parent<Database>()
                )
            );
        descriptor
            .Field(t => t.OperatorId)
            .Ignore();
        ConfigureDataField(
            descriptor,
            "data",
            _ => _.GetDataAsync(default!, default!, default, default, default!, default!, default)
        )
            .Argument("kind", _ => _.Type<NonNullType<EnumType<DataKind>>>());
        ConfigureHasDataField<DataPropositionInput>(
            descriptor,
            "hasData",
            _ => _.HasDataAsync(default!, default!, default!, default, default!, default!, default)
        )
            .Argument("kind", _ => _.Type<NonNullType<EnumType<DataKind>>>());
        ConfigureDataField(
            descriptor,
            "calorimetricData",
            _ => _.GetCalorimetricDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureAllDataField<CalorimetricDataPropositionInput>(
            descriptor,
            "allCalorimetricData",
            _ => _.GetAllCalorimetricDataAsync(default!, default, default, default, default, default, default, default!, default!, default)
        );
        ConfigureHasDataField<CalorimetricDataPropositionInput>(
            descriptor,
            "hasCalorimetricData",
            _ => _.HasCalorimetricDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureDataField(
            descriptor,
            "geometricData",
            _ => _.GetGeometricDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureAllDataField<GeometricDataPropositionInput>(
            descriptor,
            "allGeometricData",
            _ => _.GetAllGeometricDataAsync(default!, default, default, default, default, default, default, default!, default!, default)
        );
        ConfigureHasDataField<GeometricDataPropositionInput>(
            descriptor,
            "hasGeometricData",
            _ => _.HasGeometricDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureDataField(
            descriptor,
            "hygrothermalData",
            _ => _.GetHygrothermalDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureAllDataField<HygrothermalDataPropositionInput>(
            descriptor,
            "allHygrothermalData",
            _ => _.GetAllHygrothermalDataAsync(default!, default, default, default, default, default, default, default!, default!, default)
        );
        ConfigureHasDataField<HygrothermalDataPropositionInput>(
            descriptor,
            "hasHygrothermalData",
            _ => _.HasHygrothermalDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureDataField(
            descriptor,
            "lifeCycleData",
            _ => _.GetLifeCycleDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureAllDataField<LifeCycleDataPropositionInput>(
            descriptor,
            "allLifeCycleData",
            _ => _.GetAllLifeCycleDataAsync(default!, default, default, default, default, default, default, default!, default!, default)
        );
        ConfigureHasDataField<LifeCycleDataPropositionInput>(
            descriptor,
            "hasLifeCycleData",
            _ => _.HasLifeCycleDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureDataField(
            descriptor,
            "opticalData",
            _ => _.GetOpticalDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureAllDataField<OpticalDataPropositionInput>(
            descriptor,
            "allOpticalData",
            _ => _.GetAllOpticalDataAsync(default!, default, default, default, default, default, default,
                default!, default!, default)
        );
        ConfigureHasDataField<OpticalDataPropositionInput>(
            descriptor,
            "hasOpticalData",
            _ => _.HasOpticalDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureDataField(
            descriptor,
            "photovoltaicData",
            _ => _.GetPhotovoltaicDataAsync(default!, default, default, default!, default!, default)
        );
        ConfigureAllDataField<PhotovoltaicDataPropositionInput>(
            descriptor,
            "allPhotovoltaicData",
            _ => _.GetAllPhotovoltaicDataAsync(default!, default, default, default, default, default, default, default!, default!, default)
        );
        ConfigureHasDataField<PhotovoltaicDataPropositionInput>(
            descriptor,
            "hasPhotovoltaicData",
            _ => _.HasPhotovoltaicDataAsync(default!, default, default, default!, default!, default)
        );
        descriptor
            .Field("isAuthorizedToUpdateNode")
            .ResolveWith<DatabaseResolvers>(x =>
                x.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToVerifyNode")
            .ResolveWith<DatabaseResolvers>(x =>
                x.IsAuthorizedToVerifyNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private static IObjectFieldDescriptor ConfigureDataField(
        IObjectTypeDescriptor<Database> descriptor,
        string fieldName,
        Expression<Func<DatabaseResolvers, object?>> resolverMethod
    )
    {
        return descriptor
            .Field(fieldName)
            .Argument("id", _ => _.Type<NonNullType<UuidType>>())
            .Argument("locale", _ => _.Type<LocaleType>())
            .ResolveWith(resolverMethod);
    }

    private static void ConfigureAllDataField<TDataPropositionInput>(
        IObjectTypeDescriptor<Database> descriptor,
        string fieldName,
        Expression<Func<DatabaseResolvers, object?>> resolverMethod
    )
    {
        descriptor
            .Field(fieldName)
            .Argument("where", _ => _.Type<InputObjectType<TDataPropositionInput>>())
            .Argument("locale", _ => _.Type<LocaleType>())
            .Argument("first", _ => _.Type<NonNegativeIntType>())
            .Argument("after", _ => _.Type<StringType>())
            .Argument("last", _ => _.Type<NonNegativeIntType>())
            .Argument("before", _ => _.Type<StringType>())
            .ResolveWith(resolverMethod);
    }

    private static IObjectFieldDescriptor ConfigureHasDataField<TDataPropositionInput>(
        IObjectTypeDescriptor<Database> descriptor,
        string fieldName,
        Expression<Func<DatabaseResolvers, object?>> resolverMethod
    )
    {
        return descriptor
            .Field(fieldName)
            .Argument("where", _ => _.Type<InputObjectType<TDataPropositionInput>>())
            .Argument("locale", _ => _.Type<LocaleType>())
            .ResolveWith(resolverMethod);
    }
}