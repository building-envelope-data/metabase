using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

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