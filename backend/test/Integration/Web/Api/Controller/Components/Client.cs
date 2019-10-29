using ClientBase = Test.Integration.Web.Api.Controller.ClientBase;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HttpResponse = Microsoft.AspNetCore.Http.HttpResponse;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Xunit;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;
using IdentityModel.Client;
using FluentAssertions;

namespace Test.Integration.Web.Api.Controller.Components
{
    public class Client : ClientBase
    {
        public ListClient List { get; }
        public GetClient Get { get; }
        public PostClient Post { get; }

        public Client(HttpClient httpClient) : base(httpClient)
        {
            List = new ListClient(httpClient);
            Get = new GetClient(httpClient);
            Post = new PostClient(httpClient);
        }
    }

    public class ListClient : ClientBase
    {
        public class Output
        {
            public Guid Id;
        }

        public ListClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<HttpResponseMessage> Raw()
        {
            return await HttpClient.GetAsync("/api/components");
        }

        public async Task<IEnumerable<Output>> Deserialized()
        {
            return await Deserialize(await Raw());
        }

        public async Task<IEnumerable<Output>> Deserialize(HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<IEnumerable<Output>>(
                await httpResponse.Content.ReadAsStringAsync()
            );
        }
    }

    public class GetClient : ClientBase
    {
        public class Output
        {
            public Guid Id;
            public IEnumerable<VersionOutput> Versions;
        }

        public class VersionOutput
        {
          public Guid Id;
          public Guid ComponentId;
          public IEnumerable<OwnershipOutput> Ownerships;
        }

        public class OwnershipOutput
        {
          public Guid Id;
          public Guid ComponentVersionId;
          public string Name;
          public string Description;
          public string Abbreviation;
          public DateTime AvailableFrom;
          public DateTime AvailableUntil;
        }

        public GetClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<HttpResponseMessage> Raw(Guid id)
        {
            return await HttpClient.GetAsync("/api/components/" + id.ToString()); // TODO Use a better way to construct URIs. There is for example `UriBuilder`, see https://docs.microsoft.com/en-us/dotnet/api/system.uribuilder?view=netcore-3.0
        }

        public async Task<Output> Deserialized(Guid id)
        {
            return await Deserialize(await Raw(id));
        }

        public async Task<Output> Deserialize(HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Output>(
                await httpResponse.Content.ReadAsStringAsync()
            );
        }
    }

    public class PostClient : ClientBase
    {
        public class Input
        {
        }

        public PostClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<HttpResponseMessage> Raw()
        {
            return await HttpClient.PostAsync("/api/components", MakeJsonHttpContent(new Input()));
        }

        public async Task<Guid> Deserialized()
        {
            return await Deserialize(await Raw());
        }

        public async Task<Guid> Deserialize(HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Guid>(
                await httpResponse.Content.ReadAsStringAsync()
            );
        }

    }
}