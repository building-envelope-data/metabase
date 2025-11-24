using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.ContactInformations;

public sealed class ContactInformationFilterType
    : FilterInputType<ContactInformation>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ContactInformation> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(_ => _.PhoneNumber);
        descriptor.Field(_ => _.PostalAddress);
        descriptor.Field(_ => _.EmailAddress);
        descriptor.Field(_ => _.WebsiteLocator);
    }
}