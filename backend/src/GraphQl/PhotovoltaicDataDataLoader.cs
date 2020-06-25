using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class PhotovoltaicDataDataLoader
      : ModelDataLoader<PhotovoltaicData, Models.PhotovoltaicData>
    {
        public PhotovoltaicDataDataLoader(IQueryBus queryBus)
          : base(PhotovoltaicData.FromModel, queryBus)
        {
        }
    }
}