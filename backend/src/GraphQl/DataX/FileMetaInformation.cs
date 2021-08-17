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
        public Guid FormatId { get; }

        public FileMetaInformation(
          IReadOnlyList<string> path,
          Guid formatId
        )
        {
            Path = path;
            FormatId = formatId;
        }

        public Task<Metabase.Data.DataFormat?> GetDataFormatAsync(
                DataFormatByIdDataLoader dataFormatById,
                CancellationToken cancellationToken
        )
        {
            return dataFormatById.LoadAsync(
                FormatId,
                cancellationToken
                );
        }
    }
}
