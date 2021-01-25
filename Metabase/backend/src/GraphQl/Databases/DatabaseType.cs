using HotChocolate.Types;

namespace Metabase.GraphQl.Databases
{
    public sealed class DatabaseType
      : EntityType<Data.Database, DatabaseByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Database> descriptor
            )
        {
            base.Configure(descriptor);
            descriptor
                .Field(t => t.Operator)
                .ResolveWith<DatabaseResolvers>(t => t.GetOperatorAsync(default!, default!, default));
            descriptor
                .Field(t => t.OperatorId).Ignore();
        }
    }
}
