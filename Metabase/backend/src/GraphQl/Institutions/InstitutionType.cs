using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionType
      : EntityType<Data.Institution, InstitutionByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Institution> descriptor
            )
        {
            base.Configure(descriptor);
            // TODO AffiliatedPersons
            // descriptor
            //     .Field(t => t.DevelopedMethods)
            //     .ResolveWith<InstitutionResolvers>(t => t.GetDevelopedMethodsAsync(default!, default!, default!, default))
            //     .UseDbContext<Data.ApplicationDbContext>();
            descriptor
                .Field(t => t.DevelopedMethodEdges)
                .Ignore();
            descriptor
                .Field(t => t.ManufacturedComponents)
                .Resolve(context =>
                    new InstitutionManufacturedComponentConnection(
                        context.Parent<Data.Institution>()
                        )
                    );
            descriptor
                .Field(t => t.ManufacturedComponentEdges)
                .Ignore();
            descriptor
                .Field(t => t.OperatedDatabases)
                .Resolve(context =>
                    new InstitutionOperatedDatabaseConnection(
                        context.Parent<Data.Institution>()
                        )
                    );
            descriptor
                .Field(t => t.Representatives)
                .Resolve(context =>
                    new InstitutionRepresentativeConnection(
                        context.Parent<Data.Institution>()
                        )
                    );
            descriptor
                .Field(t => t.RepresentativeEdges)
                .Ignore();
        }
    }
}