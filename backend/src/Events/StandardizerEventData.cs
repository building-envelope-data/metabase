using Exception = System.Exception;

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
        public static StandardizerEventData FromModel(
            this ValueObjects.Standardizer standardizer
            )
        {
            return standardizer switch
            {
                ValueObjects.Standardizer.AERC
                    => StandardizerEventData.AERC,
                ValueObjects.Standardizer.AGI
                    => StandardizerEventData.AGI,
                ValueObjects.Standardizer.ASHRAE
                    => StandardizerEventData.ASHRAE,
                ValueObjects.Standardizer.BREEAM
                    => StandardizerEventData.BREEAM,
                ValueObjects.Standardizer.BS
                    => StandardizerEventData.BS,
                ValueObjects.Standardizer.BSI
                    => StandardizerEventData.BSI,
                ValueObjects.Standardizer.CEN
                    => StandardizerEventData.CEN,
                ValueObjects.Standardizer.CIE
                    => StandardizerEventData.CIE,
                ValueObjects.Standardizer.DGNB
                    => StandardizerEventData.DGNB,
                ValueObjects.Standardizer.DIN
                    => StandardizerEventData.DIN,
                ValueObjects.Standardizer.DVWG
                    => StandardizerEventData.DVWG,
                ValueObjects.Standardizer.IEC
                    => StandardizerEventData.IEC,
                ValueObjects.Standardizer.IES
                    => StandardizerEventData.IES,
                ValueObjects.Standardizer.IFT
                    => StandardizerEventData.IFT,
                ValueObjects.Standardizer.ISO
                    => StandardizerEventData.ISO,
                ValueObjects.Standardizer.JIS
                    => StandardizerEventData.JIS,
                ValueObjects.Standardizer.LEED
                    => StandardizerEventData.LEED,
                ValueObjects.Standardizer.NFRC
                    => StandardizerEventData.NFRC,
                ValueObjects.Standardizer.RIBA
                    => StandardizerEventData.RIBA,
                ValueObjects.Standardizer.UL
                    => StandardizerEventData.UL,
                ValueObjects.Standardizer.UNECE
                    => StandardizerEventData.UNECE,
                ValueObjects.Standardizer.VDI
                    => StandardizerEventData.VDI,
                ValueObjects.Standardizer.VFF
                    => StandardizerEventData.VFF,
                ValueObjects.Standardizer.WELL
                      => StandardizerEventData.WELL,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The standardizer {standardizer} fell through")
            };
        }

        public static ValueObjects.Standardizer ToModel(
            this StandardizerEventData standardizer
            )
        {
            return standardizer switch
            {
                StandardizerEventData.AERC
                    => ValueObjects.Standardizer.AERC,
                StandardizerEventData.AGI
                    => ValueObjects.Standardizer.AGI,
                StandardizerEventData.ASHRAE
                    => ValueObjects.Standardizer.ASHRAE,
                StandardizerEventData.BREEAM
                    => ValueObjects.Standardizer.BREEAM,
                StandardizerEventData.BS
                    => ValueObjects.Standardizer.BS,
                StandardizerEventData.BSI
                    => ValueObjects.Standardizer.BSI,
                StandardizerEventData.CEN
                    => ValueObjects.Standardizer.CEN,
                StandardizerEventData.CIE
                    => ValueObjects.Standardizer.CIE,
                StandardizerEventData.DGNB
                    => ValueObjects.Standardizer.DGNB,
                StandardizerEventData.DIN
                    => ValueObjects.Standardizer.DIN,
                StandardizerEventData.DVWG
                    => ValueObjects.Standardizer.DVWG,
                StandardizerEventData.IEC
                    => ValueObjects.Standardizer.IEC,
                StandardizerEventData.IES
                    => ValueObjects.Standardizer.IES,
                StandardizerEventData.IFT
                    => ValueObjects.Standardizer.IFT,
                StandardizerEventData.ISO
                    => ValueObjects.Standardizer.ISO,
                StandardizerEventData.JIS
                    => ValueObjects.Standardizer.JIS,
                StandardizerEventData.LEED
                    => ValueObjects.Standardizer.LEED,
                StandardizerEventData.NFRC
                    => ValueObjects.Standardizer.NFRC,
                StandardizerEventData.RIBA
                    => ValueObjects.Standardizer.RIBA,
                StandardizerEventData.UL
                    => ValueObjects.Standardizer.UL,
                StandardizerEventData.UNECE
                    => ValueObjects.Standardizer.UNECE,
                StandardizerEventData.VDI
                    => ValueObjects.Standardizer.VDI,
                StandardizerEventData.VFF
                    => ValueObjects.Standardizer.VFF,
                StandardizerEventData.WELL
                    => ValueObjects.Standardizer.WELL,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The standardizer {standardizer} fell through")
            };
        }
    }
}