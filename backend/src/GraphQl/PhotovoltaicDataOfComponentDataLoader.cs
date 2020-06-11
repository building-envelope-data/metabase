using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class PhotovoltaicDataOfComponentDataLoader
      : ForwardOneToManyAssociatesOfModelDataLoader<PhotovoltaicData, Models.Component, Models.ComponentPhotovoltaicData, Models.PhotovoltaicData>
    {
        public PhotovoltaicDataOfComponentDataLoader(IQueryBus queryBus)
          : base(PhotovoltaicData.FromModel, queryBus)
        {
        }
    }
}