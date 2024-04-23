using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public sealed class InstitutionMethodDeveloperFilterType
    : FilterInputType<InstitutionMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Method);
        descriptor.Field(x => x.Institution);
        descriptor.Field(x => x.Pending);
    }
}