using System.Collections.Generic;
using DateTime = System.DateTime;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace Metabase.Data
{
    public sealed class Component
      : Infrastructure.Data.Entity
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0
        // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0#built-in-attributes

        [MinLength(1)]
        public string Name { get; private set; }

        [MinLength(1)]
        public string? Abbreviation { get; private set; }

        [MinLength(1)]
        public string Description { get; private set; }

        public NpgsqlRange<DateTime>? Availability { get; private set; } // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_

        // https://www.npgsql.org/efcore/mapping/array.html
        public ValueObjects.ComponentCategory[] Categories { get; private set; }

        public Component(
            string name,
            string? abbreviation,
            string description,
            NpgsqlRange<DateTime>? availability,
            ValueObjects.ComponentCategory[] categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            Availability = availability;
            Categories = categories;
        }
    }
}
