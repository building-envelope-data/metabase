using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Array = System.Array;
using Console = System.Console;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateUserInput
    {
        public string Name { get; }

        public CreateUserInput(
            string name
            )
        {
            Name = name;
        }

        public static Result<ValueObjects.CreateUserInput, Errors> Validate(
            CreateUserInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.CreateUserInput.From(path);
        }
    }
}