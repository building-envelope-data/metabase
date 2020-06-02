using Console = System.Console;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;
using HotChocolate.Types;

namespace Icon.GraphQl
{
    public sealed class CreateOpticalDataInputType
      : InputObjectType<CreateOpticalDataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateOpticalDataInput> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}
