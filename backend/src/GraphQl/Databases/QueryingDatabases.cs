using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Serializer.SystemTextJson;
using IdentityModel;
using IdentityModel.Client;
using Metabase.Data;
using Metabase.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using NodaTime.Text;
using OpenIddict.Client.AspNetCore;

namespace Metabase.GraphQl.Databases;

public sealed class QueryingDatabases
{
    public const string DatabaseHttpClient = "Database";

    public static async Task<string> ConstructQuery(
        string[] fileNames
    )
    {
        return string.Join(
            Environment.NewLine,
            await Task.WhenAll(
                fileNames.Select(fileName =>
                    File.ReadAllTextAsync($"./GraphQl/Databases/Queries/{fileName}")
                )
            )
        );
    }

    private static async Task<string?> ExtractBearerToken(
        IHttpContextAccessor httpContextAccessor
    )
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        // Extract bearer token stored in cookie (used by Metabase Web
        // frontend)
        var cookieBearerToken = await httpContextAccessor.HttpContext.GetTokenAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken
        );
        if (cookieBearerToken is not null)
        {
            return cookieBearerToken;
        }

        // Extract bearer token given in authorization header (used by
        // third-party frontends)
        var bearerTokenPrefix = $"{OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer} ";
        return httpContextAccessor.HttpContext.Request?.Headers?.Authorization
            .FirstOrDefault(
                x => x is not null
                     && x.TrimStart().StartsWith(bearerTokenPrefix, StringComparison.Ordinal))
            ?.TrimStart()
            ?.Replace(bearerTokenPrefix, "");
    }

    public static async
        Task<GraphQLResponse<TGraphQlResponse>>
        QueryDatabase<TGraphQlResponse>(
            Database database,
            GraphQLRequest request,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken,
            string? apiToken = null
        )
        where TGraphQlResponse : class
    {
        // https://github.com/graphql-dotnet/graphql-client/blob/47b4abfbfda507a91b5c62a18a9789bd3a8079c7/src/GraphQL.Client/GraphQLHttpResponse.cs
        // var response =
        //   (
        //    await CreateGraphQlClient(database)
        //    .SendQueryAsync<TGraphQlResponse>(
        //      request,
        //      cancellationToken
        //      )
        //
        //    )
        //   .AsGraphQLHttpResponse();
        using var httpClient = httpClientFactory.CreateClient(DatabaseHttpClient);
        // Set the authorization header to a given API token or the bearer token
        // from the original HTTP request. Note that we cannot pass the API
        // token as well as the bearer token in one request, neither with
        // multiple HTTP authorization headers nor with one header with
        // a comma-separated list of authentication scheme and value pairs.
        // Both go against RFC 7230/7235, even though some web servers accept
        // multiple schemes. For details see
        // https://stackoverflow.com/questions/29282578/multiple-http-authorization-headers
        if (apiToken is not null)
        {
            httpClient.SetToken("Token", apiToken);
        }
        else
        {
            // We extract and set the bearer token below. Alternatively, we could
            // add a named client to the factory and set the bearer token there as
            // detailed in
            // https://stackoverflow.com/questions/51358870/configure-httpclientfactory-to-use-data-from-the-current-request-context/51460160#51460160
            var bearerToken = await ExtractBearerToken(httpContextAccessor);
            if (bearerToken is not null)
            {
                httpClient.SetBearerToken(bearerToken);
            }
        }

        // For some reason `httpClient.PostAsJsonAsync` without `MakeJsonHttpContent` but with `SerializerOptions` results in `BadRequest` status code. It has to do with `JsonContent.Create` used within `PostAsJsonAsync` --- we also cannot use `JsonContent.Create` in `MakeJsonHttpContent`. What is happening here?
        using var jsonHttpContent = MakeJsonHttpContent(request);
        using var httpResponseMessage =
            await httpClient.PostAsync(
                database.Locator,
                jsonHttpContent,
                cancellationToken
            );
        if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException(
                $"The status code is not {HttpStatusCode.OK} but {httpResponseMessage.StatusCode}.", null,
                httpResponseMessage.StatusCode);
        }

        // We could use `httpResponseMessage.Content.ReadFromJsonAsync<GraphQL.GraphQLResponse<TGraphQlResponse>>` which would make debugging more difficult though, https://docs.microsoft.com/en-us/dotnet/api/system.net.http.json.httpcontentjsonextensions.readfromjsonasync?view=net-5.0#System_Net_Http_Json_HttpContentJsonExtensions_ReadFromJsonAsync__1_System_Net_Http_HttpContent_System_Text_Json_JsonSerializerOptions_System_Threading_CancellationToken_
        using var graphQlResponseStream =
            await httpResponseMessage.Content
                .ReadAsStreamAsync(cancellationToken);
        // For debugging, the following lines of code write the response to standard output.
        // Console.WriteLine(new StreamReader(graphQlResponseStream).ReadToEnd());
        var deserializedGraphQlResponse =
            await JsonSerializer.DeserializeAsync<GraphQLResponse<TGraphQlResponse>>(
                graphQlResponseStream,
                JsonSerializerSettings.GraphQl,
                cancellationToken
            ) ?? throw new JsonException("Failed to deserialize the GraphQL response.");
        return deserializedGraphQlResponse;
    }

    // private GraphQLHttpClient CreateGraphQlClient(
    //     Data.Database database
    //     )
    // {
    //     return new GraphQLHttpClient(
    //         new GraphQLHttpClientOptions { EndPoint = database.Locator },
    //         new SystemTextJsonSerializer(SerializerOptions),
    //         _httpClientFactory.CreateClient()
    //         );
    // }

    private static ByteArrayContent MakeJsonHttpContent<TContent>(
        TContent content
    )
    {
        // For some reason using `JsonContent.Create<TContent>(content, null, SerializerOptions)` results in status code `BadRequest`.
        var result =
            new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes(
                    content,
                    JsonSerializerSettings.GraphQl
                )
            );
        result.Headers.ContentType =
            new MediaTypeHeaderValue("application/json");
        return result;
    }
}