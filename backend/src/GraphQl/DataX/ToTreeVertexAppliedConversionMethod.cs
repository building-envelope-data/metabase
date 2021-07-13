using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class ToTreeVertexAppliedConversionMethod
    {
        public ToTreeVertexAppliedConversionMethod(
        Guid methodId,
        IReadOnlyList<NamedMethodArgument> arguments,
        string sourceName
        )
        {
            MethodId = methodId;
            Arguments = arguments;
            SourceName = sourceName;
        }

        public Guid MethodId { get; }
        public IReadOnlyList<NamedMethodArgument> Arguments { get; }
        public string SourceName { get; }
    }
}
