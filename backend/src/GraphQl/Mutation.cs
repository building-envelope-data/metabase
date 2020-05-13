using System;
using GreenDonut;
using Icon.Infrastructure.Query;
using Icon.Infrastructure.Command;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Commands = Icon.Commands;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;
using HotChocolate;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using QueryException = HotChocolate.Execution.QueryException;

namespace Icon.GraphQl
{
    public sealed class Mutation
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly UserManager<Models.UserX> _userManager;

        public Mutation(ICommandBus commandBus, IQueryBus queryBus, UserManager<Models.UserX> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }

        private async Task<TPayload> Create<TInput, TValidatedInput, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<ValueObjects.TimestampedId, TPayload> newPayload
            )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.Create<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
              await _commandBus
              .Send<
              Commands.Create<TValidatedInput>,
              Result<ValueObjects.TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(timestampedId);
        }

        private async Task<TPayload> Add<TInput, TValidatedInput, TAssociation, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<TAssociation, TPayload> newPayload,
            IDataLoader<ValueObjects.TimestampedId, TAssociation> associationLoader
        )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.Add<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.Add<TValidatedInput>,
                Result<ValueObjects.TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                  await associationLoader.LoadAsync(timestampedId).ConfigureAwait(false)
                );
        }

        public Task<CreateComponentPayload> CreateComponent(
            CreateComponentInput input,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return Create<CreateComponentInput, ValueObjects.CreateComponentInput, CreateComponentPayload>(
                input,
                CreateComponentInput.Validate,
                timestampedId => new CreateComponentPayload(timestampedId)
            );
        }

        public Task<CreateDatabasePayload> CreateDatabase(
            CreateDatabaseInput input
            )
        {
            return Create<CreateDatabaseInput, ValueObjects.CreateDatabaseInput, CreateDatabasePayload>(
                input,
                CreateDatabaseInput.Validate,
                timestampedId => new CreateDatabasePayload(timestampedId)
            );
        }

        public Task<CreateInstitutionPayload> CreateInstitution(
            CreateInstitutionInput input
            )
        {
            return Create<CreateInstitutionInput, ValueObjects.CreateInstitutionInput, CreateInstitutionPayload>(
                input,
                CreateInstitutionInput.Validate,
                timestampedId => new CreateInstitutionPayload(timestampedId)
            );
        }

        public Task<CreateMethodPayload> CreateMethod(
            CreateMethodInput input
            )
        {
            return Create<CreateMethodInput, ValueObjects.CreateMethodInput, CreateMethodPayload>(
                input,
                CreateMethodInput.Validate,
                timestampedId => new CreateMethodPayload(timestampedId)
            );
        }

        public Task<CreatePersonPayload> CreatePerson(
            CreatePersonInput input
            )
        {
            return Create<CreatePersonInput, ValueObjects.CreatePersonInput, CreatePersonPayload>(
                input,
                CreatePersonInput.Validate,
                timestampedId => new CreatePersonPayload(timestampedId)
            );
        }

        public Task<CreateStandardPayload> CreateStandard(
            CreateStandardInput input
            )
        {
            return Create<CreateStandardInput, ValueObjects.CreateStandardInput, CreateStandardPayload>(
                input,
                CreateStandardInput.Validate,
                timestampedId => new CreateStandardPayload(timestampedId)
            );
        }

        public Task<AddComponentConcretizationPayload> AddComponentConcretization(
            AddComponentConcretizationInput input,
            [DataLoader] ComponentConcretizationDataLoader componentConcretizationLoader
            )
        {
            return Add<
              AddComponentConcretizationInput,
              ValueObjects.AddComponentConcretizationInput,
              ComponentConcretization,
              AddComponentConcretizationPayload
              >(
                input,
                AddComponentConcretizationInput.Validate,
                association => new AddComponentConcretizationPayload(association),
                componentConcretizationLoader
              );
        }

        public Task<AddComponentManufacturerPayload> AddComponentManufacturer(
            AddComponentManufacturerInput input,
            [DataLoader] ComponentManufacturerDataLoader componentManufacturerLoader
            )
        {
            return Add<
              AddComponentManufacturerInput,
              ValueObjects.AddComponentManufacturerInput,
              ComponentManufacturer,
              AddComponentManufacturerPayload
              >(
                input,
                AddComponentManufacturerInput.Validate,
                association => new AddComponentManufacturerPayload(association),
                componentManufacturerLoader
              );
        }

        public Task<AddInstitutionRepresentativePayload> AddInstitutionRepresentative(
            AddInstitutionRepresentativeInput input,
            [DataLoader] InstitutionRepresentativeDataLoader institutionRepresentativeLoader
            )
        {
            return Add<
              AddInstitutionRepresentativeInput,
              ValueObjects.AddInstitutionRepresentativeInput,
              InstitutionRepresentative,
              AddInstitutionRepresentativePayload
              >(
                input,
                AddInstitutionRepresentativeInput.Validate,
                association => new AddInstitutionRepresentativePayload(association),
                institutionRepresentativeLoader
              );
        }

        public Task<AddMethodDeveloperPayload> AddMethodDeveloper(
            AddMethodDeveloperInput input,
            [DataLoader] MethodDeveloperDataLoader methodDeveloperLoader
            )
        {
            return Add<
              AddMethodDeveloperInput,
              ValueObjects.AddMethodDeveloperInput,
              MethodDeveloper,
              AddMethodDeveloperPayload
              >(
                input,
                AddMethodDeveloperInput.Validate,
                association => new AddMethodDeveloperPayload(association),
                methodDeveloperLoader
              );
        }

        public Task<AddPersonAffiliationPayload> AddPersonAffiliation(
            AddPersonAffiliationInput input,
            [DataLoader] PersonAffiliationDataLoader personAffiliationLoader
            )
        {
            return Add<
              AddPersonAffiliationInput,
              ValueObjects.AddPersonAffiliationInput,
              PersonAffiliation,
              AddPersonAffiliationPayload
              >(
                input,
                AddPersonAffiliationInput.Validate,
                association => new AddPersonAffiliationPayload(association),
                personAffiliationLoader
              );
        }
    }
}