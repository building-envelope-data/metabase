using System;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Metabase.GraphQl.Components;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManufacturedComponentEdge
        : Edge<Data.Component>
    {
        public InstitutionManufacturedComponentEdge(
            Guid nodeId
        )
            : base(nodeId)
        {
        }

        public Task<Data.Component> GetNode(
            [DataLoader] ComponentByIdDataLoader componentById
            )
        {
            return componentById.LoadAsync(NodeId)!;
        }
    }
}