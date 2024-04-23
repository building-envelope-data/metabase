using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.DataFormats;

namespace Metabase.GraphQl.DataX
{
    public sealed class FileMetaInformation
    {
        public IReadOnlyList<string> Path { get; }
        public Guid DataFormatId { get; }

        public FileMetaInformation(
            IReadOnlyList<string> path,
            Guid dataFormatId
        )
        {
            Path = path;
            DataFormatId = dataFormatId;
        }

        public Task<Metabase.Data.DataFormat?> GetDataFormatAsync(
            DataFormatByIdDataLoader dataFormatById,
            CancellationToken cancellationToken
        )
        {
            return dataFormatById.LoadAsync(
                DataFormatId,
                cancellationToken
            );
        }
    }
}