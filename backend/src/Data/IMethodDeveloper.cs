using System;

namespace Metabase.Data
{
    public interface IMethodDeveloper
    {
        public Guid MethodId { get; }
        public Method Method { get; }
        public bool Pending { get; }
    }
}