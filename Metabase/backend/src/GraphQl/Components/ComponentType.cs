using HotChocolate.Types;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentType
      : EntityType<Data.Component, ComponentByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Component> descriptor
            )
        {
            base.Configure(descriptor);
            // TODO Can we use paging with custom edges with additional fields? Could we use extensions, for example,
            // [ExtendObjectType(Name = "FooEdge")]
            // public class FooEdgeExtension
            // {
            //     public int X() => ....
            // }
            descriptor
                .Field(t => t.Manufacturers)
                .ResolveWith<ComponentResolvers>(t => t.GetManufacturersAsync(default!, default!, default!, default))
                .UseDbContext<Data.ApplicationDbContext>()
                .UsePaging<InstitutionType>();
            descriptor
                .Field(t => t.ManufacturerEdges).Ignore();
        }
    }
}