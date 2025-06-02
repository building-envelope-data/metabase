using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager)
: BatchDataLoader<Guid, OpenIdConnectApplication?>(batchScheduler, options)
{
    private OpenIddictApplicationManager<OpenIdConnectApplication> _applicationManager = applicationManager;

    protected override async Task<IReadOnlyDictionary<Guid, OpenIdConnectApplication?>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var ret = new Dictionary<Guid, OpenIdConnectApplication?>();
        foreach (var key in keys)
        {
            ret.Add(key, await _applicationManager.FindByIdAsync(key.ToString(), cancellationToken: cancellationToken).ConfigureAwait(false));
        }
        return ret;
    }
}