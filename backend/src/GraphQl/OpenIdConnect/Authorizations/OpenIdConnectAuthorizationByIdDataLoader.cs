using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager)
: BatchDataLoader<Guid, OpenIdConnectAuthorization?>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, OpenIdConnectAuthorization?>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var ret = new Dictionary<Guid, OpenIdConnectAuthorization?>();
        foreach (var key in keys)
        {
            ret.Add(key, await authorizationManager.FindByIdAsync(key.ToString(), cancellationToken: cancellationToken));
        }
        return ret;
    }
}