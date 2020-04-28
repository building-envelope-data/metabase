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
      : QueryAndMutationBase
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
            [DataLoader] ComponentForTimestampedIdDataLoader componentLoader,
            IResolverContext resolverContext
            )
        {
            // TODO Use this style for all create mutations?
            return ResultHelpers.HandleFailure(
                await CreateComponentInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(async validatedInput =>
                  await Commands.CreateComponent.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
                    )
                  .Bind(async command =>
                    await (await _commandBus
                      .Send<
                        Commands.CreateComponent,
                        Result<ValueObjects.TimestampedId, Errors>
                      >(command)
                      )
                    .Map(async timestampedId =>
                      {
                          Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
                          return await componentLoader.LoadAsync(timestampedId);
                      }
                      )
                    )
                  )
                );
        }

        public async Task<Database> CreateDatabase(
            CreateDatabaseInput input,
            [DataLoader] DatabaseForTimestampedIdDataLoader databaseLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                CreateDatabaseInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(validatedInput =>
                  Commands.CreateDatabase.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await databaseLoader.LoadAsync(timestampedId);
        }

        public async Task<Institution> CreateInstitution(
            CreateInstitutionInput input,
            [DataLoader] InstitutionForTimestampedIdDataLoader institutionLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                CreateInstitutionInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(validatedInput =>
                  Commands.CreateInstitution.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await institutionLoader.LoadAsync(timestampedId);
        }

        public async Task<Method> CreateMethod(
            CreateMethodInput input,
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                CreateMethodInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                .Bind(validatedInput =>
                  Commands.CreateMethod.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await methodLoader.LoadAsync(timestampedId);
        }

        public async Task<Person> CreatePerson(
            CreatePersonInput input,
            [DataLoader] PersonForTimestampedIdDataLoader personLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                  CreatePersonInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.CreatePerson.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await personLoader.LoadAsync(timestampedId);
        }

        public async Task<Standard> CreateStandard(
            CreateStandardInput input,
            [DataLoader] StandardForTimestampedIdDataLoader standardLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                  CreateStandardInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.CreateStandard.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await standardLoader.LoadAsync(timestampedId);
        }

        /* ComponentAssembly */
        public async Task<ComponentManufacturer> AddComponentManufacturer(
            AddComponentManufacturerInput input,
            [DataLoader] ComponentManufacturerForTimestampedIdDataLoader componentManufacturerLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddComponentManufacturerInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddComponentManufacturer.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            var timestamp = timestampedId.Timestamp;
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await componentManufacturerLoader.LoadAsync(timestampedId);
        }

        public async Task<InstitutionRepresentative> AddInstitutionRepresentative(
            AddInstitutionRepresentativeInput input,
            [DataLoader] InstitutionRepresentativeForTimestampedIdDataLoader institutionRepresentativeLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddInstitutionRepresentativeInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddInstitutionRepresentative.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            var timestamp = timestampedId.Timestamp;
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            return await institutionRepresentativeLoader.LoadAsync(timestampedId);
        }

        public async Task<AddMethodDeveloperPayload> AddMethodDeveloper(
            AddMethodDeveloperInput input,
            [DataLoader] MethodDeveloperForTimestampedIdDataLoader methodDeveloperLoader,
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader,
            [DataLoader] StakeholderForTimestampedIdDataLoader stakeholderLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddMethodDeveloperInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddMethodDeveloper.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            var timestamp = timestampedId.Timestamp;
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            var methodDeveloper = await methodDeveloperLoader.LoadAsync(timestampedId);
            // We could use a resolver for method and stakeholder of the payload `AddMethodDeveloperPayload` instead of fetching method and payload here. The payload would then only have `methodId` and `stakeholderId` instead of `method` and `stakeholder`.
            return new AddMethodDeveloperPayload(
                await methodLoader.LoadAsync(
                  TimestampId(methodDeveloper.MethodId, timestamp)
                  ),
                await stakeholderLoader.LoadAsync(
                  TimestampId(methodDeveloper.StakeholderId, timestamp)
                  ),
                timestamp
                );
        }

        public async Task<AddPersonAffiliationPayload> AddPersonAffiliation(
            AddPersonAffiliationInput input,
            [DataLoader] PersonAffiliationForTimestampedIdDataLoader personAffiliationLoader,
            [DataLoader] PersonForTimestampedIdDataLoader personLoader,
            [DataLoader] InstitutionForTimestampedIdDataLoader institutionLoader,
            IResolverContext resolverContext
            )
        {
            var command = ResultHelpers.HandleFailure(
                  AddPersonAffiliationInput.Validate(input, path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddPersonAffiliation.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
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
            var timestamp = timestampedId.Timestamp;
            Timestamp.Store(timestampedId.Timestamp, resolverContext); // May be used by resolvers.
            var personAffiliation = await personAffiliationLoader.LoadAsync(timestampedId);
            // We could use a resolver for person and institution of the payload `AddPersonAffiliationPayload` instead of fetching person and payload here. The payload would then only have `personId` and `institutionId` instead of `person` and `institution`.
            return new AddPersonAffiliationPayload(
                await personLoader.LoadAsync(
                  TimestampId(personAffiliation.PersonId, timestamp)
                  ),
                await institutionLoader.LoadAsync(
                  TimestampId(personAffiliation.InstitutionId, timestamp)
                  ),
                timestamp
                );
        }
    }
}