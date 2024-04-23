using System.Collections.Generic;
using Enum = System.Enum;

namespace Metabase.GraphQl
{
    public abstract class UserErrorBase<TUserErrorCode>
        : GraphQl.IUserError
        where TUserErrorCode : struct, Enum
    {
        public TUserErrorCode Code { get; }
        public string Message { get; }
        public IReadOnlyList<string> Path { get; }

        protected UserErrorBase(
            TUserErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
        {
            Code = code;
            Message = message;
            Path = path;
        }
    }
}