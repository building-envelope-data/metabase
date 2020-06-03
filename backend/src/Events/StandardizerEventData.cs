using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Exception = System.Exception;
using Guid = System.Guid;

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
            return standardizer switch
            {
                ValueObjects.Standardizer.AERC
                    => StandardizerEventData.AERC,
                ValueObjects.Standardizer.AGI
                    => StandardizerEventData.AGI,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The standardizer {standardizer} fell through")
            };
        }

        public static ValueObjects.Standardizer ToModel(this StandardizerEventData standardizer)
        {
            return standardizer switch
            {
                StandardizerEventData.AERC
                    => ValueObjects.Standardizer.AERC,
                StandardizerEventData.AGI
                    => ValueObjects.Standardizer.AGI,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The standardizer {standardizer} fell through")
            };
        }
    }
}