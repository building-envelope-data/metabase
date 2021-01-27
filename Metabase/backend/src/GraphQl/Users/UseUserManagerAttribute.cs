using System;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Metabase.GraphQl.Users
{
    public sealed class UseUserManagerAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member
            )
        {
            descriptor.UseUserManager();
        }
    }
}