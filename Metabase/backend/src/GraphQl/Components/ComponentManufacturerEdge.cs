namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerEdge
        : HotChocolate.Types.Pagination.Edge<Data.Institution>
    {
        public ComponentManufacturerEdge(
            Data.Institution node,
            string cursor
            )
            : base(node, cursor)
        {
        }
    }
}