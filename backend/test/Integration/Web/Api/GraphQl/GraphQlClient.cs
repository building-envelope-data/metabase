// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Icon;
using ValueObjects = Icon.ValueObjects;

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

        public abstract class Payload : ResponseBase
        {
            public DateTime? requestTimestamp { get; set; }
        }

        public sealed class ComponentInformationInput
        {
            public string name { get; }
            public string? abbreviation { get; }
            public string description { get; }
            public DateTime? availableFrom { get; }
            public DateTime? availableUntil { get; }
            public ValueObjects.ComponentCategory[] categories { get; }

            public ComponentInformationInput(
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

        public sealed class CreateComponentInput
        {
            public ComponentInformationInput information { get; }

            public CreateComponentInput(ComponentInformationInput information)
            {
                this.information = information;
            }
        }

        public sealed class ComponentData : ResponseBase
        {
            public Guid? id { get; set; }
            public DateTime? timestamp { get; set; }
            public DateTime? requestTimestamp { get; set; }
            public IEnumerable<ComponentData>? versions { get; set; }
        }

        public sealed class CreateComponentPayload : Payload
        {
            public Guid? componentId { get; set; }
            public ComponentData? component { get; set; }
        }

        public sealed class CreateComponentData : ResponseBase
        {
            public CreateComponentPayload? createComponent { get; set; }
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
                )
              .ConfigureAwait(false);
            var response =
              await new ResponseParser()
              .Parse<T, Error>(httpResponse)
              .ConfigureAwait(false);
            return response;
        }

        public Task<Response<CreateComponentData, Error>> CreateComponent(
            CreateComponentInput input
            )
        {
            return Request<CreateComponentData>(
                 query: @"mutation($input: CreateComponentInput!) {
              createComponent(input: $input) {
              requestTimestamp
              component {
              id
              timestamp
              requestTimestamp
              }
              }
              }",
                 variables: new Dictionary<string, object?>()
                 {
                     ["input"] = input
                 }
                );
        }

        public async Task<CreateComponentPayload> CreateComponentSuccessfully(
            CreateComponentInput input
            )
        {
            var data =
              (await CreateComponent(input).ConfigureAwait(false))
              .EnsureSuccess();
            return
              data.createComponent
              ?? throw new InvalidOperationException($"The value of {nameof(data.createComponent)} is `null`");
        }

        public async Task<IReadOnlyList<Error>> CreateComponentErroneously(
            CreateComponentInput input
            )
        {
            return
              (await CreateComponent(input).ConfigureAwait(false))
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
            if (timestamp is null)
            {
                return Request<GetComponentsData>(
                      query: @"query {
              components {
              id
              timestamp
              requestTimestamp
              versions {
              id
              timestamp
              requestTimestamp
              }
              }
              }"
                    );
            }
            return Request<GetComponentsData>(
                  query: @"query($timestamp: DateTime) {
              components(timestamp: $timestamp) {
              id
              timestamp
              requestTimestamp
              versions {
              id
              timestamp
              requestTimestamp
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
              (await GetComponents(timestamp).ConfigureAwait(false))
              .EnsureSuccess()
              .components
              .NotNull();
        }
    }
}