using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public sealed class RemoveComponentManufacturerPayload
    {
        private readonly Data.ComponentManufacturer? _association;
        public IReadOnlyCollection<RemoveComponentManufacturerError>? Errors { get; }

        public RemoveComponentManufacturerPayload(
            Data.ComponentManufacturer componentManufacturer
            )
        {
            _association = componentManufacturer;
        }

        public RemoveComponentManufacturerPayload(
            IReadOnlyCollection<RemoveComponentManufacturerError> errors
            )
        {
            Errors = errors;
        }

        public RemoveComponentManufacturerPayload(
            RemoveComponentManufacturerError error
            )
            : this(new[] { error })
        {
        }

        public async Task<Data.Component?> GetComponentAsync(
            ComponentByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            if (_association is null)
            {
                return null;
            }
            return await byId.LoadAsync(_association.ComponentId, cancellationToken)!;
        }

        public async Task<Data.Institution?> GetInstitutionAsync(
            InstitutionByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            if (_association is null)
            {
                return null;
            }
            return await byId.LoadAsync(_association.InstitutionId, cancellationToken)!;
        }
    }
}