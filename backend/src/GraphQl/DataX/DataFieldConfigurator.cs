using System;
using System.Linq.Expressions;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX
{
    internal static class DataFieldConfigurator
    {
        internal static void ConfigureDataField<TData, TResolvers>(
            IObjectTypeDescriptor<TData> descriptor,
            string fieldName,
            Expression<Func<TResolvers, object?>> resolverMethod
        )
        {
            descriptor
                .Field(fieldName)
                .Argument("id", _ => _.Type<NonNullType<UuidType>>())
                .Argument("timestamp", _ => _.Type<DateTimeType>())
                .Argument("locale", _ => _.Type<StringType>())
                .ResolveWith(resolverMethod);
        }

        internal static void ConfigureAllDataField<TData, TResolvers, TDataPropositionInput>(
            IObjectTypeDescriptor<TData> descriptor,
            string fieldName,
            Expression<Func<TResolvers, object?>> resolverMethod
        )
        {
            descriptor
                .Field(fieldName)
                .Argument("where", _ => _.Type<NonNullType<InputObjectType<TDataPropositionInput>>>())
                .Argument("timestamp", _ => _.Type<DateTimeType>())
                .Argument("locale", _ => _.Type<StringType>())
                .Argument("first", _ => _.Type<NonNegativeIntType>())
                .Argument("after", _ => _.Type<StringType>())
                .Argument("last", _ => _.Type<NonNegativeIntType>())
                .Argument("before", _ => _.Type<StringType>())
                .ResolveWith(resolverMethod);
        }

        internal static void ConfigureHasDataField<TData, TResolvers, TDataPropositionInput>(
            IObjectTypeDescriptor<TData> descriptor,
            string fieldName,
            Expression<Func<TResolvers, object?>> resolverMethod
        )
        {
            descriptor
                .Field(fieldName)
                .Argument("where", _ => _.Type<NonNullType<InputObjectType<TDataPropositionInput>>>())
                .Argument("timestamp", _ => _.Type<DateTimeType>())
                .Argument("locale", _ => _.Type<StringType>())
                .ResolveWith(resolverMethod);
        }
    }
}