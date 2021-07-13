using System;

namespace Metabase.GraphQl.DataX
{
    public sealed class CrossDatabaseDataReference
    {
        public CrossDatabaseDataReference(
        Guid dataId,
        DateTime dataTimestamp,
        DataKind dataKind,
        Guid databaseId
        )
        {
            DataId = dataId;
            DataTimestamp = dataTimestamp;
            DataKind = dataKind;
            DatabaseId = databaseId;
        }
        public Guid DataId { get; }
        public DateTime DataTimestamp { get; }
        public DataKind DataKind { get; }
        public Guid DatabaseId { get; }
    }
}
