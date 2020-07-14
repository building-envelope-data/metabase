using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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