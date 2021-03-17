using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Json.Path;
using TokenResponse = IdentityModel.Client.TokenResponse;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;

namespace Metabase.Tests.Integration
{
    public abstract class IntegrationTests
    {
        protected CustomWebApplicationFactory Factory { get; }
        protected CollectingEmailSender EmailSender { get => Factory.EmailSender; }
        protected CollectingSmsSender SmsSender { get => Factory.SmsSender; }
        protected HttpClient HttpClient { get; }
        public const string DefaultName = "John Doe";
        public const string DefaultEmail = "john.doe@ise.fraunhofer.de";
        public const string DefaultPassword = "aaaAAA123$!@";

        protected IntegrationTests()
        {
            Factory = new CustomWebApplicationFactory();
            HttpClient = CreateHttpClient();
        }

        protected HttpClient CreateHttpClient(bool allowAutoRedirect = true)
        {
            return Factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 3,
            }
            );
        }

        protected async Task<TokenResponse> RequestAuthToken(
            string emailAddress,
            string password
            )
        {
            var response =
              await HttpClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = "http://localhost/connect/token",
                    ClientId = "metabase",
                    ClientSecret = "secret",
                    Scope = "api:read api:write api:user:manage",
                    UserName = emailAddress,
                    Password = password,
                }
              )
              .ConfigureAwait(false);
            if (response.IsError)
            {
                throw new Exception(response.Error);
            }
            return response;
        }

        protected async Task LoginUser(
            string email = DefaultEmail,
            string password = DefaultPassword
            )
        {
            var tokenResponse =
              await RequestAuthToken(
                  emailAddress: email,
                  password: password
                  )
              .ConfigureAwait(false);
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
        }

        protected void LogoutUser()
        {
            HttpClient.SetBearerToken(null);
        }

        protected async Task<string> RegisterUser(
            string name = DefaultName,
            string email = DefaultEmail,
            string password = DefaultPassword,
            string? passwordConfirmation = null
        )
        {
            return await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUser.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["name"] = name,
                    ["email"] = email,
                    ["password"] = password,
                    ["passwordConfirmation"] = passwordConfirmation ?? password
                }
                ).ConfigureAwait(false);
        }

        protected string ExtractConfirmationCodeFromEmail()
        {
            return Regex.Match(
                EmailSender.Emails.Single().Message,
                @"confirmationCode=(?<confirmationCode>\w+)"
                )
                .Groups["confirmationCode"]
                .Captures
                .Single()
                .Value;
        }

        protected string ExtractResetCodeFromEmail()
        {
            return Regex.Match(
                EmailSender.Emails.Single().Message,
                @"reset code (?<resetCode>\w+)"
                )
                .Groups["resetCode"]
                .Captures
                .Single()
                .Value;
        }

        protected async Task<string> ConfirmUserEmail(
            string confirmationCode,
            string email = DefaultEmail
            )
        {
            return await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmail.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["email"] = email,
                    ["confirmationCode"] = confirmationCode
                }
                ).ConfigureAwait(false);
        }

        protected async Task RegisterAndConfirmUser(
            string email = DefaultEmail,
            string password = DefaultPassword
        )
        {
            await RegisterUser(
                    email: email,
                    password: password
                    ).ConfigureAwait(false);
            var confirmationCode = ExtractConfirmationCodeFromEmail();
            await ConfirmUserEmail(
                confirmationCode: confirmationCode,
                email: email
                ).ConfigureAwait(false);
        }

        // protected async Task RegisterAndLoginUser(
        //     string email,
        //     string password
        // )
        // {
        //     await RegisterUser(
        //             email: email,
        //             password: password
        //             ).ConfigureAwait(false);
        //     await LoginUser(
        //         email: email,
        //         password: password
        //     ).ConfigureAwait(false);
        // }

        protected async Task RegisterAndConfirmAndLoginUser(
            string email = DefaultEmail,
            string password = DefaultPassword
        )
        {
            await RegisterAndConfirmUser(
                    email: email,
                    password: password
                    ).ConfigureAwait(false);
            await LoginUser(
                email: email,
                password: password
            ).ConfigureAwait(false);
        }

        protected Task<HttpResponseMessage> QueryGraphQl(
            string query,
            string? operationName = null,
            object? variables = null
            )
        {
            return HttpClient.PostAsync(
                "/graphql",
                MakeJsonHttpContent(
                  new GraphQlRequest(
                      query: query,
                      operationName: operationName,
                      variables: variables
                    )
                  )
                );
        }

        protected async Task<HttpContent> SuccessfullyQueryGraphQlContent(
            string query,
            string? operationName = null,
            object? variables = null
            )
        {
            var httpResponseMessage = await QueryGraphQl(
                query,
                operationName,
                variables
                ).ConfigureAwait(false);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                // We wrap this check in an if-condition such that the message
                // content is only read when the status code is not 200.
                httpResponseMessage.StatusCode.Should().Be(
                    HttpStatusCode.OK,
                    await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)
                    );
            }
            return httpResponseMessage.Content;
        }

        protected async Task<HttpContent> UnsuccessfullyQueryGraphQlContent(
            string query,
            string? operationName = null,
            object? variables = null
            )
        {
            var httpResponseMessage = await QueryGraphQl(
                query,
                operationName,
                variables
                ).ConfigureAwait(false);
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                // We wrap this check in an if-condition such that the message
                // content is only read when the status code is not 200.
                httpResponseMessage.StatusCode.Should().NotBe(
                    HttpStatusCode.OK,
                    await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)
                    );
            }
            return httpResponseMessage.Content;
        }

        protected async Task<string> SuccessfullyQueryGraphQlContentAsString(
            string query,
            string? operationName = null,
            object? variables = null
            )
        {
            return await (
               await SuccessfullyQueryGraphQlContent(
                query,
                operationName,
                variables
                )
                .ConfigureAwait(false)
            )
            .ReadAsStringAsync()
            .ConfigureAwait(false);
        }

        protected async Task<JsonElement> SuccessfullyQueryGraphQlContentAsJson(
            string query,
            string? operationName = null,
            object? variables = null
            )
        {
            return (
                await JsonDocument.ParseAsync(
                await (
               await SuccessfullyQueryGraphQlContent(
                query,
                operationName,
                variables
                )
                .ConfigureAwait(false)
            )
            .ReadAsStreamAsync()
            .ConfigureAwait(false)
            ).ConfigureAwait(false)
            ).RootElement;
        }

        protected async Task<string> UnsuccessfullyQueryGraphQlContentAsString(
            string query,
            string? operationName = null,
            object? variables = null
            )
        {
            return await (
               await UnsuccessfullyQueryGraphQlContent(
                query,
                operationName,
                variables
                )
                .ConfigureAwait(false)
            )
            .ReadAsStringAsync()
            .ConfigureAwait(false);
        }

        protected static string ExtractString(
            string jsonPath,
            JsonElement jsonElement
            )
        {
            var pathResult =
                JsonPath.Parse(jsonPath).Evaluate(
                    jsonElement
                );
            if (pathResult.Error is not null)
            {
                throw new Exception(pathResult.Error);
            }
            return pathResult.Matches?.Single()?.Value.GetString()
            ?? throw new Exception("String is null");
        }

        protected static string Base64Encode(string text)
        {
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(text)
            );
        }

        protected static string Base64Decode(string text)
        {
            return System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(text)
            );
        }

        protected void EmailsShouldContainSingle(
            string address,
            string subject,
            string messageRegEx
        )
        {
            EmailSender.Emails.Should().ContainSingle();
            var email = EmailSender.Emails.First();
            email.Address.Should().Be(address);
            email.Subject.Should().Be(subject);
            email.Message.Should().MatchRegex(messageRegEx);
        }

        protected void SmsesShouldContainSingle(
            string number,
            string messageRegEx
        )
        {
            SmsSender.Smses.Should().ContainSingle();
            var sms = SmsSender.Smses.First();
            sms.Number.Should().Be(number);
            sms.Message.Should().MatchRegex(messageRegEx);
        }

        private HttpContent MakeJsonHttpContent<TContent>(
            TContent content
            )
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new JsonStringEnumConverter());
            var result =
              new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes(
                  content,
                  options
                  )
                );
            result.Headers.ContentType =
              new MediaTypeHeaderValue("application/json");
            return result;
        }

        private sealed class GraphQlRequest
        {
            public string Query { get; }
            public string? OperationName { get; }
            public object? Variables { get; }

            public GraphQlRequest(
                string query,
                string? operationName,
                object? variables
                )
            {
                Query = query;
                OperationName = operationName;
                Variables = variables;
            }
        }
    }
}