using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
            PhoneNumber: "+99999999999",
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

    protected Task<T> GetInstitutions<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        return GetInstitutions(
            HttpClient,
            assertBefore,
            read,
            assertAfter
        );
    }

    internal static Task<T> GetInstitutions<T>(
        HttpClient httpClient,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        return QueryGraphQl(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/GetInstitutions.graphql"),
            null,
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> GetPendingInstitutions<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        return GetPendingInstitutions(
            HttpClient,
            assertBefore,
            read,
            assertAfter
        );
    }

    internal static Task<T> GetPendingInstitutions<T>(
        HttpClient httpClient,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter
    )
    {
        return QueryGraphQl(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/GetPendingInstitutions.graphql"),
            null,
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> GetInstitution<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        Guid id
    )
    {
        return GetInstitution(
            HttpClient,
            assertBefore,
            read,
            assertAfter,
            id
        );
    }

    internal static Task<T> GetInstitution<T>(
        HttpClient httpClient,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        Guid id
    )
    {
        return QueryGraphQl(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/GetInstitution.graphql"),
            new Dictionary<string, object?>
            {
                ["id"] = id
            },
            assertBefore,
            read,
            assertAfter
        );
    }

    protected Task<T> CreateInstitution<T>(
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        CreateInstitutionInput input
    )
    {
        return CreateInstitution(
            HttpClient,
            assertBefore,
            read,
            assertAfter,
            input
        );
    }

    internal static Task<T> CreateInstitution<T>(
        HttpClient httpClient,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        CreateInstitutionInput input
    )
    {
        return QueryGraphQl(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/CreateInstitution.graphql"),
            new { input },
            assertBefore,
            read,
            assertAfter
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
        return ExtractUuid(
            "$.data.createInstitution.institution.uuid",
            await CreateInstitution(
                httpClient,
                AssertHttpSuccess,
                ReadAsJson,
                AssertNoGraphQlErrors,
                input
            )
        );
    }

    protected async Task<(string, Guid)> CreateInstitutionReturningIdAndUuid(
        CreateInstitutionInput input
    )
    {
        var response = await CreateInstitution(
            AssertHttpSuccess,
            ReadAsJson,
            AssertNoGraphQlErrors,
            input
        );
        return (
            ExtractString(
                "$.data.createInstitution.institution.id",
                response
            ),
            ExtractUuid(
                "$.data.createInstitution.institution.uuid",
                response
            )
        );
    }

    internal static Task<T> VerifyInstitution<T>(
        HttpClient httpClient,
        Func<HttpResponseMessage, Task> assertBefore,
        Func<HttpResponseMessage, Task<T>> read,
        Func<T, Task> assertAfter,
        Guid institutionId
    )
    {
        return QueryGraphQl(
            httpClient,
            File.ReadAllText("Integration/GraphQl/Institutions/VerifyInstitution.graphql"),
            new Dictionary<string, object?>
            {
                ["institutionId"] = institutionId
            },
            assertBefore,
            read,
            assertAfter
        );
    }
}