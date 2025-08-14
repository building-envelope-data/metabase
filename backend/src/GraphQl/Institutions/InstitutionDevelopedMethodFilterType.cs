using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public sealed class InstitutionDevelopedMethodFilterType
    : InstitutionMethodDeveloperFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Institution).Ignore();
    }
}