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
            descriptor
                .Field(t => t.DevelopedMethods)
                .Type<NonNullType<ObjectType<InstitutionDevelopedMethodConnection>>>()
                .Resolve(context =>
                    new InstitutionDevelopedMethodConnection(
                        context.Parent<Data.Institution>()
                    )
                );
            descriptor
                .Field(t => t.DevelopedMethodEdges)
                .Ignore();
            descriptor
                .Field(t => t.ManufacturedComponents)
                .Type<NonNullType<ObjectType<InstitutionManufacturedComponentConnection>>>()
                .Resolve(context =>
                    new InstitutionManufacturedComponentConnection(
                        context.Parent<Data.Institution>()
                        )
                    );
            descriptor
                .Field(t => t.ManufacturedComponentEdges)
                .Ignore();
            descriptor
                .Field(t => t.ManagedDataFormats)
                .Type<NonNullType<ObjectType<InstitutionManagedDataFormatConnection>>>()
                .Resolve(context =>
                    new InstitutionManagedDataFormatConnection(
                        context.Parent<Data.Institution>()
                        )
                );
            descriptor
                .Field(t => t.OperatedDatabases)
                .Type<NonNullType<ObjectType<InstitutionOperatedDatabaseConnection>>>()
                .Resolve(context =>
                    new InstitutionOperatedDatabaseConnection(
                        context.Parent<Data.Institution>()
                        )
                    );
            descriptor
                .Field(t => t.Representatives)
                .Type<NonNullType<ObjectType<InstitutionRepresentativeConnection>>>()
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