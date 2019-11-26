using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using DateTime = System.DateTime;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class CreateComponentVersionManufacturer
      : CommandBase<Result<(ValueObjects.Id, ValueObjects.Timestamp), IError>>
    {
        public ValueObjects.Id ComponentVersionId { get; }
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.ComponentVersionManufacturerMarketingInformation? MarketingInformation { get; }

        private CreateComponentVersionManufacturer(
            ValueObjects.Id componentVersionId,
            ValueObjects.Id institutionId,
            ValueObjects.ComponentVersionManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            ComponentVersionId = componentVersionId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        public static Result<CreateComponentVersionManufacturer, IError> From(
            ValueObjects.Id componentVersionId,
            ValueObjects.Id institutionId,
            ValueObjects.ComponentVersionManufacturerMarketingInformation? marketingInformation,
            ValueObjects.Id creatorId
            )
        {
          return Result.Ok(
              new CreateComponentVersionManufacturer(
                componentVersionId: componentVersionId,
                institutionId: institutionId,
                marketingInformation: marketingInformation,
                creatorId: creatorId
                )
              );
        }
    }
}