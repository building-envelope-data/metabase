using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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