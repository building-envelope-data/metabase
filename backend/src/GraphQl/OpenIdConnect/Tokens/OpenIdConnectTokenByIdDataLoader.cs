using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenByIdDataLoader(
    IBatchScheduler batchScheduler,
    DataLoaderOptions options,
    OpenIddictTokenManager<OpenIdConnectToken> tokenManager)
: BatchDataLoader<Guid, OpenIdConnectToken?>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, OpenIdConnectToken?>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var ret = new Dictionary<Guid, OpenIdConnectToken?>();
        foreach (var key in keys)
        {
            ret.Add(key, await tokenManager.FindByIdAsync(key.ToString(), cancellationToken: cancellationToken));
        }
        return ret;
    }
}