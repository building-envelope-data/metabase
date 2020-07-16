using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class ComponentVariant
      : Model, IManyToManyAssociation
    {
        public Id BaseComponentId { get; }
        public Id VariantComponentId { get; }

        public Id ParentId { get => BaseComponentId; }
        public Id AssociateId { get => VariantComponentId; }

        private ComponentVariant(
            Id id,
            Id baseComponentId,
            Id variantComponentId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }

        public static Result<ComponentVariant, Errors> From(
            Id id,
            Id baseComponentId,
            Id variantComponentId,
            Timestamp timestamp
            )
        {
            return
              Result.Success<ComponentVariant, Errors>(
                  new ComponentVariant(
            id: id,
            baseComponentId: baseComponentId,
            variantComponentId: variantComponentId,
            timestamp: timestamp
            )
                  );
        }
    }
}