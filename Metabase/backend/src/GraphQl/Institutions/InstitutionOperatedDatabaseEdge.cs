using System;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Metabase.GraphQl.Databases;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionOperatedDatabaseEdge
        : Edge<Data.Database>
    {
        public InstitutionOperatedDatabaseEdge(
            Guid nodeId
        )
            : base(nodeId)
        {
        }

        public Task<Data.Database> GetNode(
            [DataLoader] DatabaseByIdDataLoader databaseById
            )
        {
            return databaseById.LoadAsync(NodeId)!;
        }
    }
}
