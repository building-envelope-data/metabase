using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseType
    : EntityType<Database, IDatabaseByIdDataLoader>
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
        descriptor
            .Field("data")
            .ResolveWith<DatabaseResolvers>(_ => _.GetDataAsync(default!, default!, default, default!, default!, default));
        descriptor
            .Field("hasData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasDataAsync(default!, default!, default!, default!, default!, default));
        descriptor
            .Field("calorimetricData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetCalorimetricDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("allCalorimetricData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetAllCalorimetricDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("hasCalorimetricData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasCalorimetricDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("geometricData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetGeometricDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("allGeometricData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetAllGeometricDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("hasGeometricData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasGeometricDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("hygrothermalData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetHygrothermalDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("allHygrothermalData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetAllHygrothermalDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("hasHygrothermalData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasHygrothermalDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("lifeCycleData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetLifeCycleDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("allLifeCycleData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetAllLifeCycleDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("hasLifeCycleData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasLifeCycleDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("opticalData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetOpticalDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("allOpticalData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetAllOpticalDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("hasOpticalData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasOpticalDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("photovoltaicData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetPhotovoltaicDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("allPhotovoltaicData")
            .ResolveWith<DatabaseResolvers>(_ => _.GetAllPhotovoltaicDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("hasPhotovoltaicData")
            .ResolveWith<DatabaseResolvers>(_ => _.HasPhotovoltaicDataAsync(default!, default, default!, default!, default));
        descriptor
            .Field("isAuthorizedToUpdateNode")
            .Cost(1)
            .ResolveWith<DatabaseResolvers>(x =>
                x.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToVerifyNode")
            .Cost(1)
            .ResolveWith<DatabaseResolvers>(x =>
                x.IsAuthorizedToVerifyNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }
}