using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class InstitutionRepresentativeDataLoader
      : ModelDataLoader<InstitutionRepresentative, Models.InstitutionRepresentative>
    {
        public InstitutionRepresentativeDataLoader(IQueryBus queryBus)
          : base(InstitutionRepresentative.FromModel, queryBus)
        {
        }
    }
}