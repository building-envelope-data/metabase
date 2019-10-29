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
        public PostClient Post { get; }

        public Client(HttpClient httpClient) : base(httpClient)
        {
            List = new ListClient(httpClient);
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