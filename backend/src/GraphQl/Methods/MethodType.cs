using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Extensions;

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
                .Field(t => t.Manager)
                .Type<NonNullType<ObjectType<MethodManagerEdge>>>()
                .Resolve(context =>
                    new MethodManagerEdge(
                        context.Parent<Data.Method>()
                        )
                    );
            descriptor
                .Field(t => t.ManagerId)
                .Ignore();
            descriptor
                .Field(t => t.Developers)
                .Argument(nameof(Data.IMethodDeveloper.Pending).FirstCharToLower(), _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
                .Type<NonNullType<ObjectType<MethodDeveloperConnection>>>()
                .Resolve(context =>
                    new MethodDeveloperConnection(
                        context.Parent<Data.Method>(),
                        context.ArgumentValue<bool?>(nameof(Data.IMethodDeveloper.Pending).FirstCharToLower()) ?? false
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