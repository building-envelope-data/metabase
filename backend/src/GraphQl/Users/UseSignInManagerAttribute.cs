using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Metabase.GraphQl.Users;

public sealed class UseSignInManagerAttribute : ObjectFieldDescriptorAttribute
{
    protected override void OnConfigure(
        IDescriptorContext context,
        IObjectFieldDescriptor descriptor,
        MemberInfo member
    )
    {
        descriptor.UseSignInManager();
    }
}