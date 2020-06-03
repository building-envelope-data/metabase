using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Exception = System.Exception;
using Guid = System.Guid;

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
            return state switch
            {
                ValueObjects.InstitutionState.UNKNOWN
                    => InstitutionStateEventData.UNKNOWN,
                ValueObjects.InstitutionState.OPERATIVE
                    => InstitutionStateEventData.OPERATIVE,
                ValueObjects.InstitutionState.INOPERATIVE
                    => InstitutionStateEventData.INOPERATIVE,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The state {state} fell through")
            };
        }

        public static ValueObjects.InstitutionState ToModel(this InstitutionStateEventData state)
        {
            return state switch
            {
                InstitutionStateEventData.UNKNOWN
                    => ValueObjects.InstitutionState.UNKNOWN,
                InstitutionStateEventData.OPERATIVE
                    => ValueObjects.InstitutionState.OPERATIVE,
                InstitutionStateEventData.INOPERATIVE
                    => ValueObjects.InstitutionState.INOPERATIVE,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The state {state} fell through")
            };
        }
    }
}