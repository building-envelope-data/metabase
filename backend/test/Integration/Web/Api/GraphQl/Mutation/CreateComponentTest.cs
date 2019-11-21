// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

using Icon;
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

namespace Test.Integration.Web.Api.GraphQl.Mutation
{
    public class CreateComponentTest : GraphQlTestBase
    {
        public CreateComponentTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Anonymously()
        {
            // Act
            var errors = await Client.CreateComponentErroneously();
            // Assert
            errors[0]?.extensions?.code?.Should().Be("AUTH_NOT_AUTHENTICATED");
        }

        [Fact]
        public async Task Authorized()
        {
            // Arrange
            // TODO Instead of seeds we should use the public API as much as possible. Otherwise the state of the database may not be a possible state in production.
            await Factory.SeedUsers();
            Factory.SeedAuth();
            await Authorize(HttpClient, "simon@icon.com", "simonSIMON123@");
            var beforeTimestamp = DateTime.UtcNow;
            // Act
            var component = await Client.CreateComponentSuccessfully();
            // Assert
            component.id.Should().NotBeEmpty();
            component.timestamp
              .Should().BeAfter(beforeTimestamp)
              .And.BeBefore(DateTime.UtcNow);
        }
    }
}