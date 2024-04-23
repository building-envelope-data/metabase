using HotChocolate.Types;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.GraphQl.OpenIdConnect
{
    public sealed class OpenIdConnectApplicationType
        : ObjectType<OpenIddictEntityFrameworkCoreApplication>
    {
        protected override void Configure(
            IObjectTypeDescriptor<OpenIddictEntityFrameworkCoreApplication> descriptor
        )
        {
            const string suffixedName = nameof(OpenIdConnectApplicationType);
            descriptor.Name(suffixedName.Remove(suffixedName.Length - "Type".Length));
        }
    }
}