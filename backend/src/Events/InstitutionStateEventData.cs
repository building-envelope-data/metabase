using Exception = System.Exception;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public enum InstitutionStateEventData
    {
        UNKNOWN,
        OPERATIVE,
        INOPERATIVE
    }

    public static class InstitutionStateEventDataExtensions
    {
        public static InstitutionStateEventData FromModel(this ValueObjects.InstitutionState state)
        {
            switch (state)
            {
                case ValueObjects.InstitutionState.UNKNOWN:
                    return InstitutionStateEventData.UNKNOWN;
                case ValueObjects.InstitutionState.OPERATIVE:
                    return InstitutionStateEventData.OPERATIVE;
                case ValueObjects.InstitutionState.INOPERATIVE:
                    return InstitutionStateEventData.INOPERATIVE;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The state {state} fell through");
        }

        public static ValueObjects.InstitutionState ToModel(this InstitutionStateEventData state)
        {
            switch (state)
            {
                case InstitutionStateEventData.UNKNOWN:
                    return ValueObjects.InstitutionState.UNKNOWN;
                case InstitutionStateEventData.OPERATIVE:
                    return ValueObjects.InstitutionState.OPERATIVE;
                case InstitutionStateEventData.INOPERATIVE:
                    return ValueObjects.InstitutionState.INOPERATIVE;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The state {state} fell through");
        }
    }
}