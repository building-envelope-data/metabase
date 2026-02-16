using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager)
: BatchDataLoader<Guid, OpenIdConnectApplication?>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, OpenIdConnectApplication?>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var ret = new Dictionary<Guid, OpenIdConnectApplication?>();
        foreach (var key in keys)
        {
            ret.Add(key, await applicationManager.FindByIdAsync(key.ToString(), cancellationToken: cancellationToken));
        }
        return ret;
    }
}