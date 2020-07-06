using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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