using System.Collections.Generic;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentVariants
{
    public sealed class AddComponentVariantPayload
    {
        public ComponentVariantOfEdge? VariantOfEdge { get; }
        public ComponentVariantOfEdge? ReverseVariantOfEdge { get; }
        public IReadOnlyCollection<AddComponentVariantError>? Errors { get; }

        public AddComponentVariantPayload(
            Data.ComponentVariant componentVariant,
            Data.ComponentVariant reverseComponentVariant
            )
        {
            VariantOfEdge = new ComponentVariantOfEdge(componentVariant);
            ReverseVariantOfEdge = new ComponentVariantOfEdge(reverseComponentVariant);
        }

        public AddComponentVariantPayload(
            IReadOnlyCollection<AddComponentVariantError> errors
            )
        {
            Errors = errors;
        }

        public AddComponentVariantPayload(
            AddComponentVariantError error
            )
            : this(new[] { error })
        {
        }
    }
}