using System.Collections.Generic;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class AddComponentAssemblyPayload
    {
        public ComponentAssembledOfEdge? ComponentAssembledOfEdge { get; }
        public ComponentPartOfEdge? ComponentPartOfEdge { get; }
        public IReadOnlyCollection<AddComponentAssemblyError>? Errors { get; }

        public AddComponentAssemblyPayload(
            Data.ComponentAssembly componentAssembly
            )
        {
            ComponentAssembledOfEdge = new ComponentAssembledOfEdge(componentAssembly);
            ComponentPartOfEdge = new ComponentPartOfEdge(componentAssembly);
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