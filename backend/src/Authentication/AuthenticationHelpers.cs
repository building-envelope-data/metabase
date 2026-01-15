using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace Metabase.Authentication;

internal static class AuthenticationHelpers
{
    // Inspired by https://github.com/dotnet/aspnetcore/blob/85565dbb30d724c955179e1989c109c677e7b263/src/Http/Http.Extensions/src/RequestHeaders.cs#L338-L342
    private static Uri? GetOrigin(RequestHeaders headers)
    {
        if (Uri.TryCreate(
            headers.Headers.Origin,
            UriKind.RelativeOrAbsolute,
            out var uri
        ))
        {
            return uri;
        }
        return null;
    }

    internal static bool IsSameOriginOrReferer(HttpRequest request)
    {
        var headers = request.GetTypedHeaders();
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/Origin#description
        var originOrReferer = GetOrigin(headers) ?? headers.Referer;
        if (originOrReferer is null)
        {
            return false;
        }
        return request.Host == HostString.FromUriComponent(originOrReferer);
    }

    internal static bool IsReferredToFromSubpath(
        HttpRequest request,
        string subpath
    )
    {
        var path = request.GetTypedHeaders().Referer?.PathAndQuery;
        return
            path is not null
            && path.Length >= subpath.Length
            && path.StartsWith(subpath, StringComparison.InvariantCulture)
            && (path.Length == subpath.Length || path[subpath.Length] == '?' || path[subpath.Length] == '/');
    }
}