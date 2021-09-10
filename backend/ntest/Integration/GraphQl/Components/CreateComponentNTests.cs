using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Metabase.GraphQl.Components;
using Metabase.NTests.Integration.GraphQl.Institutions;
using Snapshooter;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.NTests.Integration.GraphQl.Components
{
    [TestFixture]
    //[Collection(nameof(Data.Component))]
    public sealed class CreateComponentNTests
      : ComponentIntegrationNTests
    {


        [Test]
        public async Task AnonymousUser_IsAuthenticationError()
        {
            // Act
            var response =
                await UnsuccessfullyQueryGraphQlContentAsString(
                    File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
                    variables: MinimalComponentInput
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }

        [Test]
        public async Task AnonymousUser_CannotCreateComponent()
        {
            // Act
            await UnsuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Components/CreateComponent.graphql"),
                variables: MinimalComponentInput
                ).ConfigureAwait(false);
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(response);
        }
/*
        [Datapoint]
        public string values = (nameof(EnumerateComponentInputs));

        [Theory]
         //[MemberData(nameof(EnumerateComponentInputs))]
        public async Task LoggedInUser_IsSuccess(
            string key,
            CreateComponentInput input
        )
        {
            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var institutionId = await InstitutionIntegrationNTests.CreateAndVerifyInstitutionReturningUuid(
                HttpClient,
                InstitutionIntegrationNTests.PendingInstitutionInput with
                {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            // Act
            var response = await CreateComponent(
                input with
                {
                    ManufacturerId = institutionId
                }
                ).ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                SnapshotNameExtension.Create(key),
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.createComponent.component.id").Should().NotBeNullOrWhiteSpace()
                 )
                 .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.createComponent.component.uuid").Should().NotBe(Guid.Empty)
                 )
                );
        }
  */      
        /*
        [Theory]
        [MemberData(nameof(EnumerateComponentInputs))]
        public async Task LoggedInUser_CreatesComponent(
            string key,
            CreateComponentInput input
        )
        {
            // Arrange
            var userId = await RegisterAndConfirmAndLoginUser().ConfigureAwait(false);
            var institutionId = await InstitutionIntegrationTests.CreateAndVerifyInstitutionReturningUuid(
                HttpClient,
                InstitutionIntegrationTests.PendingInstitutionInput with
                {
                    OwnerIds = new[] { userId }
                }
                ).ConfigureAwait(false);
            // Act
            var (componentId, componentUuid) = await CreateComponentReturningIdAndUuid(
                input with
                {
                    ManufacturerId = institutionId
                }
                ).ConfigureAwait(false);
            var response = await GetComponents().ConfigureAwait(false);
            // Assert
            Snapshot.Match(
                response,
                SnapshotNameExtension.Create(key),
                matchOptions => matchOptions
                .Assert(fieldOptions =>
                 fieldOptions.Field<string>("data.components.edges[*].node.id").Should().Be(componentId)
                 )
                .Assert(fieldOptions =>
                 fieldOptions.Field<Guid>("data.components.edges[*].node.uuid").Should().Be(componentUuid)
                 )
                );
        }
        */
    }
}