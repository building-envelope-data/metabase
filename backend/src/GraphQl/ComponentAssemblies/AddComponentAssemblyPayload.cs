using System.Collections.Generic;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class AddComponentAssemblyPayload
    {
        public ComponentAssembledOfEdge? AssembledOfEdge { get; }
        public ComponentPartOfEdge? PartOfEdge { get; }
        public IReadOnlyCollection<AddComponentAssemblyError>? Errors { get; }

        public AddComponentAssemblyPayload(
            Data.ComponentAssembly componentAssembly
            )
        {
            AssembledOfEdge = new ComponentAssembledOfEdge(componentAssembly);
            PartOfEdge = new ComponentPartOfEdge(componentAssembly);
        }

        public AddComponentAssemblyPayload(
            IReadOnlyCollection<AddComponentAssemblyError> errors
            )
        {
            Errors = errors;
        }

        public AddComponentAssemblyPayload(
            AddComponentAssemblyError error
            )
            : this(new[] { error })
        {
        }
    }
}