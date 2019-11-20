using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class MethodDeveloper
      : Model
    {
        public Guid MethodId { get; }
        public Guid StackholderId { get; }

        public MethodDeveloper(
            Guid id,
            Guid methodId,
            Guid stackholderId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            StackholderId = stackholderId;
        }
    }
}