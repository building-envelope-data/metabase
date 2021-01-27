using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionType
      : EntityType<Data.Institution, InstitutionByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Institution> descriptor
            )
        {
            base.Configure(descriptor);
            descriptor
                .Field(t => t.OperatedDatabases)
                .ResolveWith<InstitutionResolvers>(t => t.GetOperatedDatabasesAsync(default!, default!, default!, default))
                .UseDbContext<Data.ApplicationDbContext>();
        }
    }
}