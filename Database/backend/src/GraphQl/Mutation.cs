using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using GreenDonut;
using HotChocolate;
using Infrastructure.Commands;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Errors = Infrastructure.Errors;

namespace Database.GraphQl
{
    public sealed class Mutation
      : Infrastructure.GraphQl.Mutation
    {
        public Mutation(ICommandBus commandBus, IQueryBus queryBus)
          : base(commandBus, queryBus)
        {
        }

        public Task<CreateOpticalDataPayload> CreateOpticalData(
            CreateOpticalDataInput input
            )
        {
            return Create<CreateOpticalDataInput, ValueObjects.CreateOpticalDataInput, CreateOpticalDataPayload>(
                input,
                CreateOpticalDataInput.Validate,
                timestampedId => new CreateOpticalDataPayload(timestampedId)
            );
        }

        public Task<DeleteOpticalDataPayload> DeleteOpticalData(
            DeleteOpticalDataInput input
            )
        {
            return Delete<Models.OpticalData, DeleteOpticalDataPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteOpticalDataPayload(timestampedId)
            );
        }
    }
}