using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration.GraphQl.Users
{
    public abstract class UserIntegrationTests
      : IntegrationTests
    {
        protected async Task<string> RegisterUser()
        {
            await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/RegisterUserTests/ValidData_RegistersUser.graphql")
                ).ConfigureAwait(false);
            return Regex.Match(
                EmailSender.Emails.Single().Message,
                @"confirmation code (?<confirmationCode>\w+)"
                )
                .Groups["confirmationCode"]
                .Captures
                .Single()
                .Value;
        }

        protected async Task ConfirmUserEmail(string confirmationCode)
        {
            await SuccessfullyQueryGraphQlContentAsString(
                File.ReadAllText("Integration/GraphQl/Users/ConfirmUserEmailTests/ValidData_ConfirmsUserEmail.graphql"),
                variables: new Dictionary<string, object?>
                {
                    ["confirmationCode"] = confirmationCode
                }
                ).ConfigureAwait(false);
        }
    }
}