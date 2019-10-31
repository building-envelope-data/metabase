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

    public class ListClient : ClientBase<IEnumerable<ListClient.Output>>
    {
        public class Output : OutputBase
        {
            public Guid id { get; set; }
            public int version { get; set; }
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
            return EnsureEmptyExtensionData(await Deserialize(await Raw()));
        }
    }

    public class GetClient : ClientBase<GetClient.Output>
    {
        public class Output : OutputBase
        {
            public Guid id { get; set; }
            public IEnumerable<VersionOutput> versions { get; set; }
        }

        public class VersionOutput : OutputBase
        {
            public Guid id { get; set; }
            public Guid componentId { get; set; }
            public IEnumerable<OwnershipOutput> ownerships { get; set; }
        }

        public class OwnershipOutput : OutputBase
        {
            public Guid id { get; set; }
            public Guid componentVersionId { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string abbreviation { get; set; }
            public DateTime availableFrom { get; set; }
            public DateTime availableUntil { get; set; }
        }

        public GetClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<HttpResponseMessage> Raw(Guid id)
        {
            return await HttpClient.GetAsync($"/api/components/{id}"); // TODO Use a better way to construct URIs. There is for example `UriBuilder`, see https://docs.microsoft.com/en-us/dotnet/api/system.uribuilder?view=netcore-3.0
        }

        public async Task<Output> Deserialized(Guid id)
        {
            return EnsureEmptyExtensionData(await Deserialize(await Raw(id)));
        }
    }

    public class PostClient : ClientBase<Guid>
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
    }
}