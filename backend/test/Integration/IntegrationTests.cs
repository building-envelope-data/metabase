using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using IdentityModel.Client;
using Json.Path;
using Metabase.Authentication;
using Metabase.Data;
using Metabase.Json;
using NUnit.Framework;
using Snapshooter;
using TokenResponse = IdentityModel.Client.TokenResponse;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;

namespace Metabase.Tests.Integration;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract partial class IntegrationTests
    : IDisposable
{
    public const string DefaultName = "John Doe";
    public const string DefaultEmail = "john.doe@ise.fraunhofer.de";
    public const string DefaultPassword = "aaaAAA123$!@";

    private bool _disposed;
    private CustomWebApplicationFactory Factory { get; }
    protected CollectingEmailSender EmailSender => Factory.EmailSender;
    protected AppSettings AppSettings => Factory.AppSettings;
    protected HttpClient HttpClient { get; }

    [GeneratedRegex("confirmationCode=(?<confirmationCode>\\w+)")]
    private static partial Regex ConfirmationCodeRegex();

    [GeneratedRegex("resetCode=(?<resetCode>\\w+)")]
    private static partial Regex ResetCodeRegex();

    protected IntegrationTests()
    {
        Factory = new CustomWebApplicationFactory();
        HttpClient = CreateHttpClient();
    }

    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    // https://docs.microsoft.com/en-us/dotnet/standard/managed-code
    // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
    ~IntegrationTests()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Factory.Dispose();
                HttpClient.Dispose();
            }
            _disposed = true;
        }
    }

    protected static HttpClient CreateHttpClient(
        CustomWebApplicationFactory factory,
        bool allowAutoRedirect = true
    )
    {
        var httpClient = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect,
                BaseAddress = new Uri("http://localhost", UriKind.Absolute),
                HandleCookies = true,
                MaxAutomaticRedirections = 3
            }
        );
        Task.Run(() => UpdateAntiforgeryCookieAndToken(httpClient)).Wait();
        return httpClient;
    }

    protected HttpClient CreateHttpClient(
        bool allowAutoRedirect = true
    )
    {
        return CreateHttpClient(
            Factory,
            allowAutoRedirect
        );
    }

    public Task DoAsync(Func<ApplicationDbContext, Task> what)
    {
        return Factory.DoAsync(what);
    }

    private static IEnumerable<Cookie> ExtractCookies(HttpResponseMessage response)
    {
        if (!response.Headers.TryGetValues("Set-Cookie", out var cookieEntries))
        {
            return [];
        }
        var uri = response.RequestMessage?.RequestUri ?? throw new ArgumentException($"The request URI cannot be extracted from the given response {response}.");
        var cookieContainer = new CookieContainer();
        foreach (var cookieEntry in cookieEntries)
        {
            cookieContainer.SetCookies(uri, cookieEntry);
        }
        return cookieContainer.GetCookies(uri).Cast<Cookie>();
    }

    private static async Task UpdateAntiforgeryCookieAndToken(
        HttpClient httpClient
    )
    {
        // Get the antiforgery token in the cookie "XSRF-TOKEN" and set it
        // permanently on the HTTP client by requesting /antiforgery/token
        // synchronously.
        using var response = await httpClient.GetAsync("/antiforgery/token");
        // Add the antiforgery token as the default request header "X-XSRF-TOKEN".
        var xsrfToken =
            ExtractCookies(response)
            .SingleOrDefault(cookie => cookie.Name == "XSRF-TOKEN")
            ?.Value
            ?? throw new ArgumentException("The `XSRF-TOKEN` cookie is missing in the response of a request to /antiforgery/token");
        httpClient.DefaultRequestHeaders.Remove("X-XSRF-TOKEN");
        httpClient.DefaultRequestHeaders.Add("X-XSRF-TOKEN", xsrfToken);
    }

    protected static async Task<TokenResponse> RequestAuthToken(
        HttpClient httpClient,
        string openIdConnectClientSecret,
        string emailAddress,
        string password
    )
    {
        var response =
            await httpClient.RequestPasswordTokenAsync(
                    new PasswordTokenRequest
                    {
                        Address = "http://localhost/connect/token",
                        ClientId = OpenIdConnectConstants.Client.MetabaseClientId,
                        ClientSecret = openIdConnectClientSecret,
                        Scope = "openid offline_access address email phone profile roles api:read api:write api:administrate api:verify api:database:manage api:gnu_pg:manage api:institution_representative:manage api:open_id_connect:manage api:user:manage",
                        UserName = emailAddress,
                        Password = password
                    }
                );
        if (response.IsError)
        {
            throw new HttpRequestException($"Error '{response.Error}' of type '{response.ErrorType}' with description '{response.ErrorDescription}'");
        }
        return response;
    }

    protected Task<TokenResponse> RequestAuthToken(
        string emailAddress,
        string password
    )
    {
        return RequestAuthToken(
            HttpClient,
            openIdConnectClientSecret: AppSettings.OpenIdConnectClientSecret,
            emailAddress: emailAddress,
            password: password
        );
    }

    protected static async Task LoginUser(
        HttpClient httpClient,
        string openIdConnectClientSecret,
        string emailAddress = DefaultEmail,
        string password = DefaultPassword
    )
    {
        var tokenResponse =
            await RequestAuthToken(
                    httpClient,
                    openIdConnectClientSecret: openIdConnectClientSecret,
                    emailAddress: emailAddress,
                    password: password
                );
        httpClient.SetBearerToken(tokenResponse.AccessToken ??
                                  throw new InvalidOperationException(
                                      $"The auth-token request to {httpClient.BaseAddress} with email address {emailAddress} and password {password} returned `null` as access token."));
        await UpdateAntiforgeryCookieAndToken(httpClient);
    }

    protected Task LoginUser(
        string emailAddress = DefaultEmail,
        string password = DefaultPassword
    )
    {
        return LoginUser(
            HttpClient,
            AppSettings.OpenIdConnectClientSecret,
            emailAddress: emailAddress,
            password: password
        );
    }

    protected static async Task LogoutUser(
        HttpClient httpClient
    )
    {
        httpClient.SetBearerToken("");
        await UpdateAntiforgeryCookieAndToken(httpClient);
    }

    protected Task LogoutUser()
    {
        return LogoutUser(HttpClient);
    }

    private static async Task<TResult> AsUser<TResult>(
        CustomWebApplicationFactory factory,
        string openIdConnectClientSecret,
        string emailAddress,
        string password,
        Func<HttpClient, Task<TResult>> task
    )
    {
        using var httpClient = CreateHttpClient(factory, allowAutoRedirect: true);
        await LoginUser(
            httpClient,
            openIdConnectClientSecret: openIdConnectClientSecret,
            emailAddress: emailAddress,
            password: password
        );
        var result = await task(httpClient);
        await LogoutUser(httpClient);
        return result;
    }

    protected Task<TResult> AsUser<TResult>(
        string emailAddress,
        string password,
        Func<HttpClient, Task<TResult>> task
    )
    {
        return AsUser(
            Factory,
            openIdConnectClientSecret: AppSettings.OpenIdConnectClientSecret,
            emailAddress: emailAddress,
            password: password,
            task: task
        );
    }

    protected Task<TResult> AsVerifier<TResult>(
        Func<HttpClient, Task<TResult>> task
    )
    {
        return AsUser(
            emailAddress: DbSeeder.VerifierUser.EmailAddress,
            password: AppSettings.BootstrapUserPassword,
            task: task
        );
    }

    protected Task<TResult> AsAdministrator<TResult>(
        Func<HttpClient, Task<TResult>> task
    )
    {
        return AsUser(
            emailAddress: DbSeeder.AdministratorUser.EmailAddress,
            password: AppSettings.BootstrapUserPassword,
            task: task
        );
    }

    protected async Task<T> RegisterUser<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string name = DefaultName,
        string email = DefaultEmail,
        string password = DefaultPassword,
        string? passwordConfirmation = null
    )
    {
        return await QueryGraphQl<T>(
            File.ReadAllText("Integration/GraphQl/Users/RegisterUser.graphql"),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["email"] = email,
                ["password"] = password,
                ["passwordConfirmation"] = passwordConfirmation ?? password
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected async Task<Guid> RegisterUserReturningUuid(
        string name = DefaultName,
        string email = DefaultEmail,
        string password = DefaultPassword,
        string? passwordConfirmation = null
    )
    {
        return ExtractUuid(
            "$.data.registerUser.user.uuid",
            await RegisterUser(
                HttpSuccess,
                AsJson,
                NoGraphQlErrors,
                name: name,
                email: email,
                password: password,
                passwordConfirmation: passwordConfirmation
            )
        );
    }

    protected string ExtractConfirmationCodeFromEmail()
    {
        return ConfirmationCodeRegex()
            .Match(EmailSender.Emails.Single().Body)
            .Groups["confirmationCode"]
            .Captures
            .Single()
            .Value;
    }

    protected string ExtractResetCodeFromEmail()
    {
        return ResetCodeRegex()
            .Match(EmailSender.Emails.Single().Body)
            .Groups["resetCode"]
            .Captures
            .Single()
            .Value;
    }

    protected Task<T> ConfirmUserEmail<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        string confirmationCode,
        string email = DefaultEmail
    )
    {
        return QueryGraphQl(
            File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmail.graphql"),
            new Dictionary<string, object?>
            {
                ["email"] = email,
                ["confirmationCode"] = confirmationCode
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected async Task<Guid> RegisterAndConfirmUser(
        string name = DefaultName,
        string email = DefaultEmail,
        string password = DefaultPassword
    )
    {
        var uuid =
            await RegisterUserReturningUuid(
                name,
                email,
                password
            );
        var confirmationCode = ExtractConfirmationCodeFromEmail();
        await ConfirmUserEmail(
            HttpSuccess,
            AsJson,
            NoGraphQlErrors,
            confirmationCode: confirmationCode,
            email: email
        );
        return uuid;
    }

    protected async Task<Guid> RegisterAndConfirmAndLoginUser(
        string name = DefaultName,
        string email = DefaultEmail,
        string password = DefaultPassword
    )
    {
        var uuid =
            await RegisterAndConfirmUser(
                name,
                email,
                password
            );
        await LoginUser(
            email,
            password
        );
        return uuid;
    }

    protected Task<T> QueryGraphQl<T>(
        string query,
        object? variables,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        return QueryGraphQl(
            HttpClient,
            query,
            variables,
            assertBefore,
            read,
            assertAfter
        );
    }

    protected static async Task<T> QueryGraphQl<T>(
        HttpClient httpClient,
        string query,
        object? variables,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        using var response = await httpClient.PostAsync(
            "/graphql",
            MakeJsonHttpContent(
                new GraphQlRequest(
                    query,
                    variables
                )
            )
        );
        if (assertBefore is not null)
        {
            await assertBefore(response);
        }
        var readResponse = await read(response);
        if (assertAfter is not null)
        {
            await assertAfter(readResponse);
        }
        return readResponse;
    }

    protected static async Task HttpSuccess(HttpResponseMessage message)
    {
        if (message.StatusCode != HttpStatusCode.OK)
        {
            // We wrap this check in an if-condition such that the message
            // content is only read when the status code is not 200.
            message.StatusCode.Should().Be(
                HttpStatusCode.OK,
                await message.Content.ReadAsStringAsync()
            );
        }
    }

    protected static async Task HttpFailure(HttpResponseMessage message)
    {
        if (message.StatusCode == HttpStatusCode.OK)
        {
            // We wrap this check in an if-condition such that the message
            // content is only read when the status code is not 200.
            message.StatusCode.Should().NotBe(
                HttpStatusCode.OK,
                await message.Content.ReadAsStringAsync()
            );
        }
    }

    protected static async Task<JsonElement> AsJson(HttpResponseMessage message)
    {
        using var document = await JsonDocument.ParseAsync(
            await message.Content.ReadAsStreamAsync()
        );
        return document.RootElement.Clone();
    }

    protected static Task<string> AsString(HttpResponseMessage message)
    {
        return message.Content.ReadAsStringAsync();
    }

    protected static Task ForSnapshotMatch(string content)
    {
        // There is nothing to assert.
        return Task.CompletedTask;
    }

    protected static Task NoGraphQlErrors(JsonElement root)
    {
        using (new AssertionScope())
        {
            // { "errors": [...], "data": { "<queryName>": { "errors": [...] } } }
            if (root.TryGetProperty("errors", out JsonElement errorsElement))
            {
                errorsElement.ValueKind.Should().Be(
                    JsonValueKind.Null,
                    $"The GraphQL response has errors: {errorsElement.GetRawText()}"
                );
            }
            if (root.TryGetProperty("data", out JsonElement dataElement))
            {
                if (dataElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in dataElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Object)
                        {
                            if (property.Value.TryGetProperty("errors", out JsonElement userErrorsElement))
                            {
                                userErrorsElement.ValueKind.Should().Be(
                                    JsonValueKind.Null,
                                    $"The GraphQL response has user errors: {userErrorsElement.GetRawText()}"
                                );
                            }
                        }
                    }
                }
            }
        }
        return Task.CompletedTask;
    }

    protected static Task HasGraphQlErrors(JsonElement root)
    {
        // { "errors": [...], "data": { "<queryName>": { "errors": [...] } } }
        var hasErrors = false;
        if (root.TryGetProperty("errors", out JsonElement errorsElement))
        {
            hasErrors = true;
        }
        if (!hasErrors && root.TryGetProperty("data", out JsonElement dataElement))
        {
            if (dataElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in dataElement.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Object)
                    {
                        if (property.Value.TryGetProperty("errors", out JsonElement userErrorsElement))
                        {
                            if (userErrorsElement.ValueKind != JsonValueKind.Null)
                            {
                                hasErrors = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        hasErrors.Should().BeTrue("The GraphQL response does not have errors.");
        return Task.CompletedTask;
    }

    protected static string ExtractString(
        string jsonPath,
        JsonElement jsonElement
    )
    {
        var pathResult =
            JsonPath.Parse(jsonPath).Evaluate(
                JsonObject.Create(jsonElement)
            );

        return pathResult.Matches?.Single()?.Value?.GetValue<string>()
               ?? throw new ArgumentException("String is null");
    }

    protected static Guid ExtractUuid(
        string jsonPath,
        JsonElement jsonElement
    )
    {
        return new Guid(
            ExtractString(
                jsonPath,
                jsonElement
            )
        );
    }

    protected static string Base64Encode(string text)
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes(text)
        );
    }

    protected static string Base64Decode(string text)
    {
        return Encoding.UTF8.GetString(
            Convert.FromBase64String(text)
        );
    }

    protected void EmailsShouldContainSingle(
        (string name, string address) recipient,
        string subject,
        string bodyRegEx
    )
    {
        EmailSender.Emails.Should().ContainSingle();
        var email = EmailSender.Emails.First();
        email.Recipient.Should().Be(recipient);
        email.Subject.Should().Be(subject);
        email.Body.Should().MatchRegex(bodyRegEx);
    }

    private static ByteArrayContent MakeJsonHttpContent<TContent>(
        TContent content
    )
    {
        var result =
            new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes(
                    content,
                    JsonSerializerSettings.GraphQl
                )
            );
        result.Headers.ContentType =
            new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
        return result;
    }

    // With NUnit using async Snapshooter is not able to calculate
    // the necessary Fullname, due to reasons mentioned in
    // https://stackoverflow.com/questions/22598323/movenext-instead-of-actual-method-task-name
    // The workaround with optional parameters is inspired by the same source.
    protected static SnapshotFullName SnapshotFullNameHelper(
        Type testType,
        string keyName,
        [CallerMemberName] string testMethod = "",
        [CallerFilePath] string testFilePath = ""
    )
    {
        var testName = $"{testType.Name}.{testMethod}_{keyName}.snap";
        var testDirectory =
            Path.GetDirectoryName(testFilePath)
            ?? throw new ArgumentException($"The path '{testFilePath}' denotes a root directory or is `null`.");
        return new SnapshotFullName(testName, testDirectory);
    }

    private sealed class GraphQlRequest(
        string query,
        object? variables
        )
    {
        public string Query { get; } = query;
        public object? Variables { get; } = variables;
    }
}