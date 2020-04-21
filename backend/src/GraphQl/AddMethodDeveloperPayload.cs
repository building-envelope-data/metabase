using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class AddMethodDeveloperPayload
      : Payload
    {
        public Method? Method { get; }
        public Stakeholder? Stakeholder { get; }

        public AddMethodDeveloperPayload(
            Method? method,
            Stakeholder? stakeholder,
            DateTime timestamp
            )
          : base(timestamp)
        {
            Method = method;
            Stakeholder = stakeholder;
        }
    }
}