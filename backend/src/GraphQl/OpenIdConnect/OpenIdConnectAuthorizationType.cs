using HotChocolate.Types;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.GraphQl.OpenIdConnect
{
    public sealed class OpenIdConnectAuthorizationType
      : ObjectType<OpenIddictEntityFrameworkCoreAuthorization>
    {
        protected override void Configure(
            IObjectTypeDescriptor<OpenIddictEntityFrameworkCoreAuthorization> descriptor
            )
        {
            const string suffixedName = nameof(OpenIdConnectAuthorizationType);
            descriptor.Name(suffixedName.Remove(suffixedName.Length - "Type".Length));
        }
    }
}