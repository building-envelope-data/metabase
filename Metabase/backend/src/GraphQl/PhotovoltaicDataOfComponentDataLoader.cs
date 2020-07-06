using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class PhotovoltaicDataOfComponentDataLoader
      : DataOfComponentDataLoader<Models.PhotovoltaicData, PhotovoltaicData>
    {
        public PhotovoltaicDataOfComponentDataLoader(IQueryBus queryBus)
          : base(PhotovoltaicData.FromModel, queryBus)
        {
        }
    }
}