using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.ContactInformations;

public sealed class ContactInformationSortType
    : SortInputType<ContactInformation>
{
    protected override void Configure(
        ISortInputTypeDescriptor<ContactInformation> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(_ => _.PhoneNumber);
        descriptor.Field(_ => _.PostalAddress);
        descriptor.Field(_ => _.EmailAddress);
        descriptor.Field(_ => _.WebsiteLocator);
    }
}