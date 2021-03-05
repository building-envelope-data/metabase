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
            descriptor
            .Field(t => t.Developers)
                .Type<NonNullType<ObjectType<MethodDeveloperConnection>>>()
                .Resolve(context =>
                    new MethodDeveloperConnection(
                        context.Parent<Data.Method>()
                    )
                );
            descriptor
            .Field(t => t.InstitutionDevelopers)
            .Ignore();
            descriptor
            .Field(t => t.InstitutionDeveloperEdges)
            .Ignore();
            descriptor
            .Field(t => t.UserDevelopers)
            .Ignore();
            descriptor
            .Field(t => t.UserDeveloperEdges)
            .Ignore();
        }
    }
}