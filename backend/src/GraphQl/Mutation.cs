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
        private readonly UserManager<Models.User> _userManager;

        public Mutation(ICommandBus commandBus, IQueryBus queryBus, UserManager<Models.User> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }

        public async Task<Component> CreateComponent(
            CreateComponentInput input,
            [DataLoader] ComponentForTimestampedIdDataLoader componentLoader
            )
        {
            // TODO Use this style for all create mutations?
            return ResultHelpers.HandleFailure(
                await CreateComponentInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(async validatedInput =>
                  await Commands.CreateComponent.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.New() // TODO Use current user!
                    )
                  .Bind(async command =>
                    await (await _commandBus
                      .Send<
                        Commands.CreateComponent,
                        Result<ValueObjects.TimestampedId, Errors>
                      >(command)
                      )
                    .Map(async timestampedId =>
                      await componentLoader.LoadAsync(timestampedId)
                      )
                    )
                  )
                );
        }

        public async Task<Database> CreateDatabase(
            CreateDatabaseInput input,
            [DataLoader] DatabaseForTimestampedIdDataLoader databaseLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                CreateDatabaseInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(validatedInput =>
                  Commands.CreateDatabase.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.New() // TODO Use current user!
                    )
                  )
                );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.CreateDatabase,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return await databaseLoader.LoadAsync(timestampedId);
        }

        public async Task<Institution> CreateInstitution(
            CreateInstitutionInput input,
            [DataLoader] InstitutionForTimestampedIdDataLoader institutionLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                CreateInstitutionInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(validatedInput =>
                  Commands.CreateInstitution.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.New() // TODO Use current user!
                    )
                  )
                );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.CreateInstitution,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return await institutionLoader.LoadAsync(timestampedId);
        }

        public async Task<Method> CreateMethod(
            CreateMethodInput input,
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                CreateMethodInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(validatedInput =>
                  Commands.CreateMethod.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.New() // TODO Use current user!
                    )
                  )
                );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.CreateMethod,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return await methodLoader.LoadAsync(timestampedId);
        }

        public async Task<Person> CreatePerson(
            CreatePersonInput input,
            [DataLoader] PersonForTimestampedIdDataLoader personLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                  CreatePersonInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.CreatePerson.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
              await _commandBus
              .Send<
              Commands.CreatePerson,
              Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return await personLoader.LoadAsync(timestampedId);
        }

        public async Task<Standard> CreateStandard(
            CreateStandardInput input,
            [DataLoader] StandardForTimestampedIdDataLoader standardLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                  CreateStandardInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.CreateStandard.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
              await _commandBus
              .Send<
              Commands.CreateStandard,
              Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return await standardLoader.LoadAsync(timestampedId);
        }

        /* TODO ComponentAssembly */

        public async Task<AddComponentManufacturerPayload> AddComponentManufacturer(
            AddComponentManufacturerInput input,
            [DataLoader] ComponentManufacturerForTimestampedIdDataLoader componentManufacturerLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddComponentManufacturerInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddComponentManufacturer.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.AddComponentManufacturer,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return new AddComponentManufacturerPayload(
                await componentManufacturerLoader.LoadAsync(timestampedId)
                );
        }

        public async Task<AddInstitutionRepresentativePayload> AddInstitutionRepresentative(
            AddInstitutionRepresentativeInput input,
            [DataLoader] InstitutionRepresentativeForTimestampedIdDataLoader institutionRepresentativeLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddInstitutionRepresentativeInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddInstitutionRepresentative.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.AddInstitutionRepresentative,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return new AddInstitutionRepresentativePayload(
                await institutionRepresentativeLoader.LoadAsync(timestampedId)
                );
        }

        public async Task<AddMethodDeveloperPayload> AddMethodDeveloper(
            AddMethodDeveloperInput input,
            [DataLoader] MethodDeveloperForTimestampedIdDataLoader methodDeveloperLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddMethodDeveloperInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddMethodDeveloper.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.AddMethodDeveloper,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return new AddMethodDeveloperPayload(
                await methodDeveloperLoader.LoadAsync(timestampedId)
                );
        }

        public async Task<AddPersonAffiliationPayload> AddPersonAffiliation(
            AddPersonAffiliationInput input,
            [DataLoader] PersonAffiliationForTimestampedIdDataLoader personAffiliationLoader
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddPersonAffiliationInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddPersonAffiliation.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.AddPersonAffiliation,
                Result<ValueObjects.TimestampedId, Errors>
                >(command)
                );
            return new AddPersonAffiliationPayload(
                await personAffiliationLoader.LoadAsync(timestampedId)
                );
        }
    }
}