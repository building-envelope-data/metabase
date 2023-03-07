using System.Collections.Generic;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class UpdateComponentAssemblyPayload
    {
        public ComponentAssembledOfEdge? ComponentAssembledOfEdge { get; }
        public ComponentPartOfEdge? ComponentPartOfEdge { get; }
        public IReadOnlyCollection<UpdateComponentAssemblyError>? Errors { get; }

        public UpdateComponentAssemblyPayload(
            Data.ComponentAssembly componentAssembly
            )
        {
            ComponentAssembledOfEdge = new ComponentAssembledOfEdge(componentAssembly);
            ComponentPartOfEdge = new ComponentPartOfEdge(componentAssembly);
        }

        public UpdateComponentAssemblyPayload(
            IReadOnlyCollection<UpdateComponentAssemblyError> errors
            )
        {
            Errors = errors;
        }

        public UpdateComponentAssemblyPayload(
            UpdateComponentAssemblyError error
            )
            : this(new[] { error })
        {
        }
    }
}