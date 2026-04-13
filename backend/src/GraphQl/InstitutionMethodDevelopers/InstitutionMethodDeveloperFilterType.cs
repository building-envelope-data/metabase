using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public abstract class InstitutionMethodDeveloperFilterType
    : FilterInputType<InstitutionMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Method);
        descriptor.Field(x => x.Institution);
    }
}