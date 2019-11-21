// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

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

namespace Test.Integration.Web.Api.GraphQl
{
    public class GraphQlClient
    {
        protected static HttpContent MakeJsonHttpContent<TContent>(TContent content)
        {
            var result =
              new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes<TContent>(
                  content
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

        public class ComponentData : ResponseBase
        {
            public Guid? id { get; set; }
            public DateTime? timestamp { get; set; }
            public IEnumerable<ComponentVersionData>? versions { get; set; }
        }

        public class ComponentVersionData : ResponseBase
        {
            public Guid? id { get; set; }
            public Guid? componentId { get; set; }
            public DateTime? timestamp { get; set; }
        }

        public class CreateComponentData : ResponseBase
        {
            public ComponentData? createComponent { get; set; }
        }

        public async Task<Response<T, Error>> Request<T>(
            string query,
            string? operationName = null,
            Dictionary<string, object>? variables = null
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

        public Task<Response<CreateComponentData, Error>> CreateComponent()
        {
            return Request<CreateComponentData>(
                 @"mutation {
              createComponent {
              id
              timestamp
              versions {
              id
              componentId
              timestamp
              }
              }
              }"
                );
        }

        public async Task<ComponentData> CreateComponentSuccessfully()
        {
            var data =
              (await CreateComponent())
              .EnsureSuccess();
            return data.createComponent ?? throw new InvalidOperationException($"The value of {nameof(data.createComponent)} is `null`");
        }

        public async Task<IReadOnlyList<Error>> CreateComponentErroneously()
        {
            return
              (await CreateComponent())
              .EnsureFailure();
        }

        public class GetComponentsData : ResponseBase
        {
            public IEnumerable<ComponentData>? components { get; set; }
        }

        public Task<Response<GetComponentsData, Error>> GetComponents()
        {
            return Request<GetComponentsData>(
                  @"query {
              components {
              id
              timestamp
              versions {
              id
              componentId
              timestamp
              }
              }
              }"
                );
        }

        public async Task<IEnumerable<ComponentData>> GetComponentsSuccessfully()
        {
            return
              (await GetComponents())
              .EnsureSuccess()
              .components
                            .NotNull();
        }
    }
}