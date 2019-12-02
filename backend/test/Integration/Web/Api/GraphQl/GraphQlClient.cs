// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;
using Icon;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using HttpResponse = Microsoft.AspNetCore.Http.HttpResponse;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Xunit;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;
using IdentityModel.Client;
using FluentAssertions;
using System.Text.Json.Serialization;

namespace Test.Integration.Web.Api.GraphQl
{
    public class GraphQlClient
    {
        protected static HttpContent MakeJsonHttpContent<TContent>(TContent content)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            var result =
              new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes<TContent>(
                  content,
                  options
                  )
                );
            result.Headers.ContentType =
              new MediaTypeHeaderValue("application/json");
            return result;
        }

        private readonly HttpClient _httpClient;

        public GraphQlClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public sealed class ComponentInputData
        {
            public string name { get; }
            public string? abbreviation { get; }
            public string description { get; }
            public DateTime? availableFrom { get; }
            public DateTime? availableUntil { get; }
            public ValueObjects.ComponentCategory[] categories { get; }

            public ComponentInputData(
                string name,
                string? abbreviation,
                string description,
                DateTime? availableFrom,
                DateTime? availableUntil,
                ValueObjects.ComponentCategory[] categories
                )
            {
                this.name = name;
                this.abbreviation = abbreviation;
                this.description = description;
                this.availableFrom = availableFrom;
                this.availableUntil = availableUntil;
                this.categories = categories;
            }
        }

        public sealed class ComponentData : ResponseBase
        {
            public Guid? id { get; set; }
            public DateTime? timestamp { get; set; }
            public IEnumerable<ComponentVersionData>? versions { get; set; }
        }

        public sealed class ComponentVersionData : ResponseBase
        {
            public Guid? id { get; set; }
            public Guid? componentId { get; set; }
            public DateTime? timestamp { get; set; }
        }

        public sealed class CreateComponentData : ResponseBase
        {
            public ComponentData? createComponent { get; set; }
        }

        public async Task<Response<T, Error>> Request<T>(
            string query,
            string? operationName = null,
            Dictionary<string, object?>? variables = null
            )
          where T : ResponseBase
        {
            var httpResponse = await _httpClient.PostAsync(
                "/graphql",
                MakeJsonHttpContent(
                  new Request(
                      query: query,
                      operationName: operationName,
                      variables: variables
                    )
                  )
                );
            var response =
              await new ResponseParser()
              .Parse<T, Error>(httpResponse);
            return response;
        }

        public Task<Response<CreateComponentData, Error>> CreateComponent(
            ComponentInputData input
            )
        {
            return Request<CreateComponentData>(
                 query: @"mutation($input: ComponentInput!) {
              createComponent(input: $input) {
              id
              timestamp
              versions {
              id
              componentId
              timestamp
              }
              }
              }",
                 variables: new Dictionary<string, object?>()
                 {
                     ["input"] = input
                 }
                );
        }

        public async Task<ComponentData> CreateComponentSuccessfully(
            ComponentInputData input
            )
        {
            var data =
              (await CreateComponent(input))
              .EnsureSuccess();
            return
              data.createComponent
              ?? throw new InvalidOperationException($"The value of {nameof(data.createComponent)} is `null`");
        }

        public async Task<IReadOnlyList<Error>> CreateComponentErroneously(
            ComponentInputData input
            )
        {
            return
              (await CreateComponent(input))
              .EnsureFailure();
        }

        public sealed class GetComponentsData : ResponseBase
        {
            public IEnumerable<ComponentData>? components { get; set; }
        }

        public Task<Response<GetComponentsData, Error>> GetComponents(
            DateTime? timestamp = null
            )
        {
            return Request<GetComponentsData>(
                  query: @"query($timestamp: DateTime) {
              components(timestamp: $timestamp) {
              id
              timestamp
              versions {
              id
              componentId
              timestamp
              }
              }
              }",
                  variables: new Dictionary<string, object?>()
                  {
                      ["timestamp"] = timestamp
                  }
                );
        }

        public async Task<IEnumerable<ComponentData>> GetComponentsSuccessfully(
            DateTime? timestamp = null
            )
        {
            return
              (await GetComponents(timestamp))
              .EnsureSuccess()
              .components
              .NotNull();
        }
    }
}