using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using IdentityModel.Client;
using Metabase.Authentication;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using OpenIddict.Client;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Abstractions.OpenIddictExceptions;

namespace Metabase.Services;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Failed to create access token for database {DatabaseId}.")]
    public static partial void FailedToCreateAccessToken(this ILogger<QueryingDatabases> logger, Guid databaseId, Exception exception);
}

public sealed class QueryingDatabases(
    IHttpContextAccessor httpContextAccessor,
    IHttpClientFactory httpClientFactory,
    OpenIddictClientService clientService,
    AppSettings appSettings,
    ILogger<QueryingDatabases> logger
)
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

    /// Use the database as audience and resource such that it can use
    /// the access token to extract the user ID, ask the metabase for
    /// a user with that ID, and do some authorization with the publicly
    /// available information like the institutions the user represents.
    /// However, the access token cannot be used to get non-public
    /// information because the resource is NOT the metabase and
    /// therefore for example the query `currentUser` does not work with
    /// this access token.
    private async Task<string?> CreateRestrictedAccessTokenForDatabaseAsync(
        Database database,
        CancellationToken cancellationToken
    )
    {
        var subjectAccessToken = httpContextAccessor.HttpContext?.ExtractBearerToken();
        if (subjectAccessToken is null)
        {
            return null;
        }
        try
        {
            List<string> audiences = [OpenIdConnectConstants.Client.MetabaseClientId];
            List<string> scopes = [];
            List<string> resources = [appSettings.GraphQlEndpoint.AbsoluteUri];
            // Use client services https://documentation.openiddict.com/guides/getting-started/integrating-with-a-remote-server-instance#implement-a-non-interactive-oauth-2-0-client-in-any-net-application
            // Terrible hacks would have been https://github.com/openiddict/openiddict-core/issues/1241#issuecomment-2379027128
            // as stated by Kevin in https://github.com/openiddict/openiddict-core/issues/1241#issuecomment-2379132924
            var metabaseAuthenticationResult = await clientService.AuthenticateWithClientCredentialsAsync(
                new()
                {
                    RegistrationId = OpenIdConnectConstants.Client.MetabaseRegistrationId,
                    Scopes = scopes,
                    Resources = resources,
                    CancellationToken = cancellationToken,
                }
            );
            // How to use token exchange https://github.com/openiddict/openiddict-core/issues/1249#issuecomment-2801477016
            // and where it was implemented https://github.com/openiddict/openiddict-core/pull/2335
            // For its specification see https://oauth.net/2/token-exchange/
            // Possible audiences, scopes and resources are the unions of the
            // ones of the subject token and actor token.
            // A good read on token exchange is https://zitadel.com/docs/guides/integrate/token-exchange
            //
            // The Token Exchange grant implements RFC 8693, OAuth 2.0 Token
            // Exchange and can be used to exchange tokens to a different
            // scope, audience or subject. Changing the subject of an
            // authenticated token is called impersonation or delegation.
            var subjectAuthenticationResult = await clientService.AuthenticateWithTokenExchangeAsync(
                new()
                {
                    RegistrationId = OpenIdConnectConstants.Client.MetabaseRegistrationId,
                    RequestedTokenType = TokenTypeIdentifiers.AccessToken,
                    SubjectToken = subjectAccessToken, // identity of the party on behalf of whom the request is being made
                    SubjectTokenType = TokenTypeIdentifiers.AccessToken,
                    ActorToken = metabaseAuthenticationResult.AccessToken, // identity of the acting party: the impersonator
                    ActorTokenType = TokenTypeIdentifiers.AccessToken,
                    Audiences = audiences,
                    Scopes = scopes,
                    Resources = resources,
                    CancellationToken = cancellationToken,
                }
            );
            return subjectAuthenticationResult.IssuedToken;
        }
        catch (ProtocolException exception)
        {
            logger.FailedToCreateAccessToken(database.Id, exception);
            return null;
        }
    }

    public async
        Task<GraphQLResponse<TGraphQlResponse>>
        QueryDatabase<TGraphQlResponse>(
            Database database,
            GraphQLRequest request,
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
            // Create and set restricted access token that can be used to
            // identify the authenticated user.
            var accessToken = await CreateRestrictedAccessTokenForDatabaseAsync(
                database,
                cancellationToken
            );
            if (accessToken is not null)
            {
                httpClient.SetBearerToken(accessToken);
            }
        }
        // For some reason `httpClient.PostAsJsonAsync` without `MakeJsonHttpContent` but with `SerializerOptions` results in `BadRequest` status code. It has to do with `JsonContent.Create` used within `PostAsJsonAsync` --- we also cannot use `JsonContent.Create` in `MakeJsonHttpContent`. What is happening here?
        using var jsonHttpContent = MakeJsonHttpContent(request);
        jsonHttpContent.Headers.Add(
            HeaderNames.Origin,
            appSettings.Uri.AbsoluteUri
        );
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
            new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json);
        return result;
    }
}