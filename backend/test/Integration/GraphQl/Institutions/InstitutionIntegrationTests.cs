using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.ContactInformations;
using Metabase.GraphQl.Institutions;

namespace Metabase.Tests.Integration.GraphQl.Institutions;

public abstract class InstitutionIntegrationTests
    : IntegrationTests
{
    internal static CreateInstitutionInput PendingInstitutionInput { get; } = new(
        null,
        "Institution A",
        "I!A",
        "Best institution ever!",
        new ContactInformationInput(
            PhoneNumber: "(999) 9999-9999",
            PostalAddress: "Street 9, 7777 Town",
            EmailAddress: "aaa@institution.com",
            WebsiteLocator: new Uri("https://institution-a.com", UriKind.Absolute)
        ),
        null,
        [],
        null
    );

    internal static CreateInstitutionInput CustomIdInstitutionInput { get; } = new(
        Guid.NewGuid(),
        "Institution B",
        "I!B",
        "Custom ID institution.",
        new ContactInformationInput(
            PhoneNumber: null,
            PostalAddress: null,
            EmailAddress: "bbb@institution.com",
            WebsiteLocator: new Uri("https://institution-b.com", UriKind.Absolute)
        ),
        null,
        [],
        null
    );

    internal static IEnumerable<CreateInstitutionInput> InstitutionInputs
    {
        get
        {
            yield return PendingInstitutionInput;
            yield return CustomIdInstitutionInput;
        }
    }

    internal static IEnumerable<object[]> EnumerateInstitutionInputs()
    {
        yield return new object[] { nameof(PendingInstitutionInput), PendingInstitutionInput };
        yield return new object[] { nameof(CustomIdInstitutionInput), CustomIdInstitutionInput };
    }

    protected Task<string> GetInstitutions()
    {
        return GetInstitutions(HttpClient);
    }

    internal static Task<string> GetInstitutions(
        HttpClient httpClient
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/GetInstitutions.graphql")
        );
    }

    protected Task<string> GetPendingInstitutions()
    {
        return GetPendingInstitutions(HttpClient);
    }

    internal static Task<string> GetPendingInstitutions(
        HttpClient httpClient
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/GetPendingInstitutions.graphql")
        );
    }

    protected Task<string> GetInstitution(
        Guid id
    )
    {
        return GetInstitution(HttpClient, id);
    }

    internal static Task<string> GetInstitution(
        HttpClient httpClient,
        Guid id
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/GetInstitution.graphql"),
            variables: new Dictionary<string, object?>
            {
                ["id"] = id
            }
        );
    }

    protected Task<string> CreateInstitution(
        CreateInstitutionInput input
    )
    {
        return CreateInstitution(HttpClient, input);
    }

    internal static Task<string> CreateInstitution(
        HttpClient httpClient,
        CreateInstitutionInput input
    )
    {
        return SuccessfullyQueryGraphQlContentAsString(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
            variables: input
        );
    }

    protected Task<JsonElement> CreateInstitutionAsJson(
        CreateInstitutionInput input
    )
    {
        return CreateInstitutionAsJson(HttpClient, input);
    }

    internal static Task<JsonElement> CreateInstitutionAsJson(
        HttpClient httpClient,
        CreateInstitutionInput input
    )
    {
        return SuccessfullyQueryGraphQlContentAsJson(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
            variables: input
        );
    }

    protected Task<Guid> CreateInstitutionReturningUuid(
        CreateInstitutionInput input
    )
    {
        return CreateInstitutionReturningUuid(HttpClient, input);
    }

    internal static async Task<Guid> CreateInstitutionReturningUuid(
        HttpClient httpClient,
        CreateInstitutionInput input
    )
    {
        var response = await CreateInstitutionAsJson(httpClient, input);
        return new Guid(
            ExtractString(
                "$.data.createInstitution.institution.uuid",
                response
            )
        );
    }

    internal static async Task<Guid> CreateAndVerifyInstitutionReturningUuid(
        HttpClient httpClient,
        string verifierPassword,
        CreateInstitutionInput input
    )
    {
        var uuid = await CreateInstitutionReturningUuid(httpClient, input);
        await VerifyInstitutionByVerifierUser(httpClient, verifierPassword, uuid);
        return uuid;
    }

    protected async Task<(string, string)> CreateInstitutionReturningIdAndUuid(
        CreateInstitutionInput input
    )
    {
        var response = await CreateInstitutionAsJson(input);
        return (
            ExtractString(
                "$.data.createInstitution.institution.id",
                response
            ),
            ExtractString(
                "$.data.createInstitution.institution.uuid",
                response
            )
        );
    }

    internal static Task<string> VerifyInstitutionByVerifierUser(
        HttpClient httpClient,
        string verifierPassword,
        Guid institutionId
    )
    {
        return AsUser(
            httpClient,
            DbSeeder.VerifierUser.EmailAddress,
            verifierPassword,
            httpClient =>
            {
                return SuccessfullyQueryGraphQlContentAsString(
                    httpClient,
                    File.ReadAllText("Integration/GraphQl/Institutions/VerifyInstitution.graphql"),
                    variables: new Dictionary<string, object?>
                    {
                        ["institutionId"] = institutionId
                    }
                );
            }
        );
    }
}