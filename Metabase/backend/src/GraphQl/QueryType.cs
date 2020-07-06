using HotChocolate.Types;

namespace Metabase.GraphQl
{
    public sealed class QueryType
      : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            /* descriptor */
            /*   .Field(f => f.GetOpticalData(null!, null!, null!, null!)) */
            /*   .Type<NonNullType<ListType<NonNullType<AnyType>>>>(); */
        }
    }
}