using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class Institution
      : Stakeholder
    {
        public InstitutionInformation Information { get; }
        public string? PublicKey { get; }
        public InstitutionState State { get; }

        public Institution(
            Guid id,
            InstitutionInformation information,
            string? publicKey,
            InstitutionState state,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Information = information;
            PublicKey = publicKey;
            State = state;
        }
    }
}