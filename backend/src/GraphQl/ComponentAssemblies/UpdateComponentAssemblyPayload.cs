using System.Collections.Generic;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class UpdateComponentAssemblyPayload
    {
        public ComponentAssembledOfEdge? AssembledOfEdge { get; }
        public ComponentPartOfEdge? PartOfEdge { get; }
        public IReadOnlyCollection<UpdateComponentAssemblyError>? Errors { get; }

        public UpdateComponentAssemblyPayload(
            Data.ComponentAssembly componentAssembly
            )
        {
            AssembledOfEdge = new ComponentAssembledOfEdge(componentAssembly);
            PartOfEdge = new ComponentPartOfEdge(componentAssembly);
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