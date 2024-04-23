using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionFilterType
    : FilterInputType<Data.Institution>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Data.Institution> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Abbreviation);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.WebsiteLocator);
        descriptor.Field(x => x.State);
        descriptor.Field(x => x.DevelopedMethods);
        descriptor.Field(x => x.DevelopedMethodEdges);
        descriptor.Field(x => x.ManagedMethods);
        descriptor.Field(x => x.ManagedDataFormats);
        descriptor.Field(x => x.ManufacturedComponents);
        descriptor.Field(x => x.ManufacturedComponentEdges);
        descriptor.Field(x => x.OperatedDatabases);
        descriptor.Field(x => x.Manager);
        descriptor.Field(x => x.ManagedInstitutions);
        descriptor.Field(x => x.Representatives);
        descriptor.Field(x => x.RepresentativeEdges);
    }
}