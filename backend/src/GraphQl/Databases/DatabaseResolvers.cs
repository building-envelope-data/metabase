using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.DataX;
using Metabase.GraphQl.CalorimetricDataX;
using Metabase.GraphQl.GeometricDataX;
using Metabase.GraphQl.HygrothermalDataX;
using Metabase.GraphQl.LifeCycleDataX;
using Metabase.GraphQl.OpticalDataX;
using Metabase.GraphQl.PhotovoltaicDataX;
using Metabase.GraphQl.Requests;
using Metabase.GraphQl.Scalars;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseResolvers(
    DataQueries dataQueries
)
{
    public Task<bool> IsAuthorizedToUpdateNodeAsync(
        [Parent] Database database,
        ClaimsPrincipal claimsPrincipal,
        DatabaseAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToUpdate(claimsPrincipal, database.Id, cancellationToken);
    }

    public Task<bool> IsAuthorizedToVerifyNodeAsync(
        [Parent] Database database,
        ClaimsPrincipal claimsPrincipal,
        DatabaseAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToVerify(claimsPrincipal, database.Id, cancellationToken);
    }

    public Task<IData?> GetDataAsync(
        [Parent] Database database,
        Guid id,
        DataKind kind,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetDataAsync(database, id, kind, locale, resolverContext, cancellationToken);
    }

    public Task<bool?> HasDataAsync(
        [Parent] Database database,
        DataKind kind,
        DataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasDataAsync(database, kind, where, locale, resolverContext, cancellationToken);
    }

    public Task<CalorimetricData?> GetCalorimetricDataAsync(
        [Parent] Database database,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetCalorimetricDataAsync(database, id, locale, resolverContext, cancellationToken);
    }

    public Task<GeometricData?> GetGeometricDataAsync(
        [Parent] Database database,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetGeometricDataAsync(database, id, locale, resolverContext, cancellationToken);
    }

    public Task<HygrothermalData?> GetHygrothermalDataAsync(
        [Parent] Database database,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetHygrothermalDataAsync(database, id, locale, resolverContext, cancellationToken);
    }

    public Task<LifeCycleData?> GetLifeCycleDataAsync(
        [Parent] Database database,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetLifeCycleDataAsync(database, id, locale, resolverContext, cancellationToken);
    }

    public Task<OpticalData?> GetOpticalDataAsync(
        [Parent] Database database,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetOpticalDataAsync(database, id, locale, resolverContext, cancellationToken);
    }

    public Task<PhotovoltaicData?> GetPhotovoltaicDataAsync(
        [Parent] Database database,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetPhotovoltaicDataAsync(database, id, locale, resolverContext, cancellationToken);
    }

    public Task<CalorimetricDataConnection?> GetAllCalorimetricDataAsync(
        [Parent] Database database,
        CalorimetricDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllCalorimetricDataAsync(database, where, locale, first, after, last, before, resolverContext, cancellationToken);
    }

    public Task<GeometricDataConnection?> GetAllGeometricDataAsync(
        [Parent] Database database,
        GeometricDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllGeometricDataAsync(database, where, locale, first, after, last, before, resolverContext, cancellationToken);
    }

    public Task<HygrothermalDataConnection?> GetAllHygrothermalDataAsync(
        [Parent] Database database,
        HygrothermalDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllHygrothermalDataAsync(database, where, locale, first, after, last, before, resolverContext, cancellationToken);
    }

    public Task<LifeCycleDataConnection?> GetAllLifeCycleDataAsync(
        [Parent] Database database,
        LifeCycleDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllLifeCycleDataAsync(database, where, locale, first, after, last, before, resolverContext, cancellationToken);
    }

    public Task<OpticalDataConnection?> GetAllOpticalDataAsync(
        [Parent] Database database,
        OpticalDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllOpticalDataAsync(database, where, locale, first, after, last, before, resolverContext, cancellationToken);
    }

    public Task<PhotovoltaicDataConnection?> GetAllPhotovoltaicDataAsync(
        [Parent] Database database,
        PhotovoltaicDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllPhotovoltaicDataAsync(database, where, locale, first, after, last, before, resolverContext, cancellationToken);
    }

    public Task<bool?> HasCalorimetricDataAsync(
        [Parent] Database database,
        CalorimetricDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasCalorimetricDataAsync(database, where, locale, resolverContext, cancellationToken);
    }

    public Task<bool?> HasGeometricDataAsync(
        [Parent] Database database,
        GeometricDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasGeometricDataAsync(database, where, locale, resolverContext, cancellationToken);
    }

    public Task<bool?> HasHygrothermalDataAsync(
        [Parent] Database database,
        HygrothermalDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasHygrothermalDataAsync(database, where, locale, resolverContext, cancellationToken);
    }

    public Task<bool?> HasLifeCycleDataAsync(
        [Parent] Database database,
        LifeCycleDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasLifeCycleDataAsync(database, where, locale, resolverContext, cancellationToken);
    }

    public Task<bool?> HasOpticalDataAsync(
        [Parent] Database database,
        OpticalDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasOpticalDataAsync(database, where, locale, resolverContext, cancellationToken);
    }

    public Task<bool?> HasPhotovoltaicDataAsync(
        [Parent] Database database,
        PhotovoltaicDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasPhotovoltaicDataAsync(database, where, locale, resolverContext, cancellationToken);
    }
}