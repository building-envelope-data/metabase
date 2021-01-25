using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodType
      : EntityType<Data.Method, MethodByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Method> descriptor
            )
        {
            base.Configure(descriptor);
        }
    }
}
