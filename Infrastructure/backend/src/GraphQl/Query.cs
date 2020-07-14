using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.GraphQl
{
    public abstract class Query
    {
        protected readonly IQueryBus _queryBus;

        protected Query(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }
    }
}