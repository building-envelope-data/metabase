using Console = System.Console;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

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