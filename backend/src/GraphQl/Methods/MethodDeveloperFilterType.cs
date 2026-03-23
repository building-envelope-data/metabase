using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperFilterType
    : FilterInputType<IMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<IMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        // Disjunctively compose the filters following filters taking into
        // account the "lifting" done in `MethodDeveloperConnection`.
        // descriptor
        //     .Field(nameof(InstitutionMethodDeveloper.Institution))
        //     .Type<InstitutionFilterType>();
        // descriptor
        //     .Field(nameof(UserMethodDeveloper.User))
        //     .Type<UserFilterType>();
    }
}