using Exception = System.Exception;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public enum StandardizerEventData
    {
        AERC,
        AGI,
        ASHRAE,
        BREEAM,
        BS,
        BSI,
        CEN,
        CIE,
        DGNB,
        DIN,
        DVWG,
        IEC,
        IES,
        IFT,
        ISO,
        JIS,
        LEED,
        NFRC,
        RIBA,
        UL,
        UNECE,
        VDI,
        VFF,
        WELL
    }

    public static class StandardizerEventDataExtensions
    {
        // TODO Convert all other standardizers!
        public static StandardizerEventData FromModel(this ValueObjects.Standardizer standardizer)
        {
            switch (standardizer)
            {
                case ValueObjects.Standardizer.AERC:
                    return StandardizerEventData.AERC;
                case ValueObjects.Standardizer.AGI:
                    return StandardizerEventData.AGI;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The standardizer {standardizer} fell through");
        }

        public static ValueObjects.Standardizer ToModel(this StandardizerEventData standardizer)
        {
            switch (standardizer)
            {
                case StandardizerEventData.AERC:
                    return ValueObjects.Standardizer.AERC;
                case StandardizerEventData.AGI:
                    return ValueObjects.Standardizer.AGI;
            }
            // God-damned C# does not have switch expression exhaustiveness for
            // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
            throw new Exception($"The standardizer {standardizer} fell through");
        }
    }
}