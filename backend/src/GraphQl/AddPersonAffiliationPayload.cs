using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class AddPersonAffiliationPayload
      : Payload
    {
        public Person Person { get; }
        public Institution Institution { get; }

        public AddPersonAffiliationPayload(
            Person person,
            Institution institution,
            DateTime timestamp
            )
          : base(timestamp)
        {
            Person = person;
            Institution = institution;
        }
    }
}