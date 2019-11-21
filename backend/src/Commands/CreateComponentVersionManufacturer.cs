using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class CreateComponentVersionManufacturer
      : CommandBase<Result<(Guid Id, DateTime Timestamp), IError>>
    {
        public Guid ComponentVersionId { get; }
        public Guid InstitutionId { get; }
        public Models.ComponentVersionManufacturerMarketingInformation? MarketingInformation { get; }

        public CreateComponentVersionManufacturer(
            Guid componentVersionId,
            Guid institutionId,
            Models.ComponentVersionManufacturerMarketingInformation? marketingInformation,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentVersionId = componentVersionId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              ComponentVersionId != Guid.Empty &&
              InstitutionId != Guid.Empty &&
              (MarketingInformation?.IsValid() ?? true);
        }
    }
}