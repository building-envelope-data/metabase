using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class ComponentPart
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id AssembledComponentId { get; }
        public ValueObjects.Id PartComponentId { get; }

        public ValueObjects.Id ParentId { get => AssembledComponentId; }
        public ValueObjects.Id AssociateId { get => PartComponentId; }

        private ComponentPart(
            ValueObjects.Id id,
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }

        public static Result<ComponentPart, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentPart, Errors>(
                  new ComponentPart(
            id: id,
            assembledComponentId: assembledComponentId,
            partComponentId: partComponentId,
            timestamp: timestamp
            )
                  );
        }
    }
}