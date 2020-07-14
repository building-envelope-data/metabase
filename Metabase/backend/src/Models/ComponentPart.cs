using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class ComponentPart
      : Model, IManyToManyAssociation
    {
        public Id AssembledComponentId { get; }
        public Id PartComponentId { get; }

        public Id ParentId { get => AssembledComponentId; }
        public Id AssociateId { get => PartComponentId; }

        private ComponentPart(
            Id id,
            Id assembledComponentId,
            Id partComponentId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }

        public static Result<ComponentPart, Errors> From(
            Id id,
            Id assembledComponentId,
            Id partComponentId,
            Timestamp timestamp
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