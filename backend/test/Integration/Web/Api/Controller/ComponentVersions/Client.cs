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

namespace Test.Integration.Web.Api.Controller.ComponentVersions
{
    public class Client : ClientBase
    {
        public PostClient Post { get; }

        public Client(HttpClient httpClient) : base(httpClient)
        {
            Post = new PostClient(httpClient);
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

        public async Task<HttpResponseMessage> Raw(Guid componentId)
        {
            return await HttpClient.PostAsync($"/api/components/{componentId}/versions", MakeJsonHttpContent(new Input()));
        }

        public async Task<Guid> Deserialized(Guid componentId)
        {
            return await Deserialize(await Raw(componentId));
        }
    }
}