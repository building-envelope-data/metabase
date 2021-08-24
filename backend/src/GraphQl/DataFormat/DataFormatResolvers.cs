
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class DataFormatResolvers
    {
        public async Task<DataX.OpticalData?> GetOpticalDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return null;
        }

        public async Task<DataX.OpticalDataConnection?> GetAllOpticalDataAsync(
            Data.Database database,
            DataX.OpticalDataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            uint? first,
            string? after,
            uint? last,
            string? before,
            AllOpticalDataDataLoader dataLoader,
            CancellationToken cancellationToken
            )
        {
            return dataLoader.LoadAsync(cancellationToken);
        }

        public async Task<bool?> GetHasOpticalDataAsync(
            Data.Database database,
            DataX.DataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
            )
        {
            return null;
        }
    }
}