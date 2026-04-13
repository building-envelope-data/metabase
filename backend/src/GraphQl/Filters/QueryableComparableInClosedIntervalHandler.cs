using System;
using System.Linq.Expressions;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Utilities;

namespace Metabase.GraphQl.Filters;

public sealed class QueryableComparableInClosedIntervalHandler<T>
: QueryableComparableOperationHandler
where T : notnull
{
    public QueryableComparableInClosedIntervalHandler(
        ITypeConverter typeConverter,
        InputParser inputParser)
        : base(typeConverter, inputParser)
    {
        CanBeNull = false;
    }

    // This is used to match the handler to all `inClosedInterval` fields
    protected override int Operation => AdditionalFilterOperations.InClosedInterval;

    public override Expression HandleOperation(
        QueryableFilterContext context,
        IFilterOperationField field,
        IValueNode value,
        object? parsedValue
    )
    {
        // The context's instance is the expression path to the property
        // e.g. ~> y.gValue
        var propertyPath = context.GetInstance();
        // The parsed value is what was specified in the query
        // e.g. ~> { lowerBound: 0.0, upperBound: 1.0 }
        ArgumentNullException.ThrowIfNull(parsedValue);
        if (parsedValue is ClosedIntervalInput<T> closedIntervalInput)
        {
            // Creates and returns the LINQ operation
            // e.g. ~> 0.0 >= y.gValue && y.gValue <= 1.0
            return Expression.AndAlso(
                FilterExpressionBuilder.GreaterThanOrEqual(propertyPath, closedIntervalInput.LowerBound),
                FilterExpressionBuilder.LowerThanOrEqual(propertyPath, closedIntervalInput.UpperBound)
            );
        }
        // Something went wrong
        throw new InvalidOperationException();
    }
}