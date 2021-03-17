using HotChocolate.Types;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.GraphQl.OpenIdConnect
{
    public sealed class OpenIdConnectScopeType
      : ObjectType<OpenIddictEntityFrameworkCoreScope>
    {
        protected override void Configure(
            IObjectTypeDescriptor<OpenIddictEntityFrameworkCoreScope> descriptor
            )
        {
            var suffixedName = nameof(OpenIdConnectScopeType);
            descriptor.Name(suffixedName.Remove(suffixedName.Length - "Type".Length));
        }
    }
}