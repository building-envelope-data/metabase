using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class Institution
      : Stakeholder
    {
        public Guid InformationId { get; }
        public string? PublicKey { get; }
        public InstitutionState State { get; }

        public Institution(
            Guid id,
            Guid informationId,
            string? publicKey,
            InstitutionState state,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            InformationId = informationId;
            PublicKey = publicKey;
            State = state;
        }
    }
}