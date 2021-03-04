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
            descriptor
            .Field(t => t.Standard)
            .Ignore();
            descriptor
            .Field(t => t.Publication)
            .Ignore();
            // TODO Fetch `Reference` efficiently. Is it always pre-loaded automatically because it is owned or do we need a data loader?
            // TODO Use connections for relations.
        }
    }
}