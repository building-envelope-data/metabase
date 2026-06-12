using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.ContactInformations;

public sealed class ContactInformationType
    : ObjectType<ContactInformation>
{
    protected override void Configure(
        IObjectTypeDescriptor<ContactInformation> descriptor
    )
    {
        descriptor
            .Field(_ => _.EmailAddress)
            .Type<EmailAddressType>();
        descriptor
            .Field(_ => _.PhoneNumber)
            .Type<PhoneNumberType>();
    }
}