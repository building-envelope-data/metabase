using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using GreenDonut;
using HotChocolate;
using Icon.Infrastructure.Commands;
using Icon.Infrastructure.Queries;
using Microsoft.AspNetCore.Identity;

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

        private async Task<TPayload> Delete<TModel, TPayload>(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            Func<ValueObjects.TimestampedId, TPayload> newPayload
            )
        {
            var command = ResultHelpers.HandleFailure(
                  ValueObjects.TimestampedId.From(id, timestamp, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(timestampedId =>
                    Commands.Delete<TModel>.From(
                      timestampedId: timestampedId,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
              await _commandBus
              .Send<
              Commands.Delete<TModel>,
              Result<ValueObjects.TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(timestampedId);
        }

        private async Task<TPayload> AddAssociation<TInput, TValidatedInput, TAssociation, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<TAssociation, TPayload> newPayload,
            IDataLoader<ValueObjects.TimestampedId, TAssociation> associationLoader
        )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddAssociation<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.AddAssociation<TValidatedInput>,
                Result<ValueObjects.TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                  await associationLoader.LoadAsync(timestampedId).ConfigureAwait(false)
                );
        }

        private async Task<TPayload> RemoveManyToManyAssociation<TAssociationModel, TInput, TValidatedInput, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<ValueObjects.Id, ValueObjects.Id, ValueObjects.Timestamp, TPayload> newPayload
        )
          where TValidatedInput : ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.RemoveAssociation<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.RemoveAssociation<TValidatedInput>,
                Result<ValueObjects.TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                command.Input.ParentId,
                command.Input.AssociateId,
                timestampedId.Timestamp
                );
        }

        private async Task<TPayload> RemoveOneToManyAssociation<TAssociationModel, TInput, TValidatedInput, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<ValueObjects.Id, ValueObjects.Timestamp, TPayload> newPayload
        )
          where TValidatedInput : ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.RemoveAssociation<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: ValueObjects.Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.RemoveAssociation<TValidatedInput>,
                Result<ValueObjects.TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                command.Input.AssociateId,
                timestampedId.Timestamp
                );
        }

        public Task<CreateComponentPayload> CreateComponent(
            CreateComponentInput input
            )
        {
            return Create<CreateComponentInput, ValueObjects.CreateComponentInput, CreateComponentPayload>(
                input,
                CreateComponentInput.Validate,
                timestampedId => new CreateComponentPayload(timestampedId)
            );
        }

        public Task<DeleteComponentPayload> DeleteComponent(
            DeleteComponentInput input
            )
        {
            return Delete<Models.Component, DeleteComponentPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteComponentPayload(timestampedId)
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

        public Task<DeleteDatabasePayload> DeleteDatabase(
            DeleteDatabaseInput input
            )
        {
            return Delete<Models.Database, DeleteDatabasePayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteDatabasePayload(timestampedId)
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

        public Task<DeleteInstitutionPayload> DeleteInstitution(
            DeleteInstitutionInput input
            )
        {
            return Delete<Models.Institution, DeleteInstitutionPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteInstitutionPayload(timestampedId)
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

        public Task<DeleteMethodPayload> DeleteMethod(
            DeleteMethodInput input
            )
        {
            return Delete<Models.Method, DeleteMethodPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteMethodPayload(timestampedId)
            );
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

        public Task<DeletePersonPayload> DeletePerson(
            DeletePersonInput input
            )
        {
            return Delete<Models.Person, DeletePersonPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeletePersonPayload(timestampedId)
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

        public Task<DeleteStandardPayload> DeleteStandard(
            DeleteStandardInput input
            )
        {
            return Delete<Models.Standard, DeleteStandardPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteStandardPayload(timestampedId)
            );
        }

        public Task<CreateUserPayload> CreateUser(
            CreateUserInput input
            )
        {
            return Create<CreateUserInput, ValueObjects.CreateUserInput, CreateUserPayload>(
                input,
                CreateUserInput.Validate,
                timestampedId => new CreateUserPayload(timestampedId)
            );
        }

        public Task<DeleteUserPayload> DeleteUser(
            DeleteUserInput input
            )
        {
            return Delete<Models.User, DeleteUserPayload>(
                input.Id,
                input.Timestamp,
                timestampedId => new DeleteUserPayload(timestampedId)
            );
        }

        public Task<AddComponentConcretizationPayload> AddComponentConcretization(
            AddComponentConcretizationInput input,
            [DataLoader] ComponentConcretizationDataLoader componentConcretizationLoader
            )
        {
            return AddAssociation<
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

        public Task<RemoveComponentConcretizationPayload> RemoveComponentConcretization(
            RemoveComponentConcretizationInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.ComponentConcretization,
              RemoveComponentConcretizationInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentConcretization>,
              RemoveComponentConcretizationPayload
              >(
                input,
                RemoveComponentConcretizationInput.Validate,
                (generalComponentId, concreteComponentId, requestTimestamp) => new RemoveComponentConcretizationPayload(generalComponentId, concreteComponentId, requestTimestamp)
              );
        }

        public Task<AddComponentManufacturerPayload> AddComponentManufacturer(
            AddComponentManufacturerInput input,
            [DataLoader] ComponentManufacturerDataLoader componentManufacturerLoader
            )
        {
            return AddAssociation<
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

        public Task<RemoveComponentManufacturerPayload> RemoveComponentManufacturer(
            RemoveComponentManufacturerInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.ComponentManufacturer,
              RemoveComponentManufacturerInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentManufacturer>,
              RemoveComponentManufacturerPayload
              >(
                input,
                RemoveComponentManufacturerInput.Validate,
                (componentId, institutionId, requestTimestamp) => new RemoveComponentManufacturerPayload(componentId, institutionId, requestTimestamp)
              );
        }

        public Task<AddComponentPartPayload> AddComponentPart(
            AddComponentPartInput input,
            [DataLoader] ComponentPartDataLoader componentPartLoader
            )
        {
            return AddAssociation<
              AddComponentPartInput,
              ValueObjects.AddComponentPartInput,
              ComponentPart,
              AddComponentPartPayload
              >(
                input,
                AddComponentPartInput.Validate,
                association => new AddComponentPartPayload(association),
                componentPartLoader
              );
        }

        public Task<RemoveComponentPartPayload> RemoveComponentPart(
            RemoveComponentPartInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.ComponentPart,
              RemoveComponentPartInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>,
              RemoveComponentPartPayload
              >(
                input,
                RemoveComponentPartInput.Validate,
                (assembledComponentId, partComponentId, requestTimestamp) => new RemoveComponentPartPayload(assembledComponentId, partComponentId, requestTimestamp)
              );
        }

        public Task<AddComponentVariantPayload> AddComponentVariant(
            AddComponentVariantInput input,
            [DataLoader] ComponentVariantDataLoader componentVariantLoader
            )
        {
            return AddAssociation<
              AddComponentVariantInput,
              ValueObjects.AddComponentVariantInput,
              ComponentVariant,
              AddComponentVariantPayload
              >(
                input,
                AddComponentVariantInput.Validate,
                association => new AddComponentVariantPayload(association),
                componentVariantLoader
              );
        }

        public Task<RemoveComponentVariantPayload> RemoveComponentVariant(
            RemoveComponentVariantInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.ComponentVariant,
              RemoveComponentVariantInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVariant>,
              RemoveComponentVariantPayload
              >(
                input,
                RemoveComponentVariantInput.Validate,
                (baseComponentId, variantComponentId, requestTimestamp) => new RemoveComponentVariantPayload(baseComponentId, variantComponentId, requestTimestamp)
              );
        }

        public Task<AddComponentVersionPayload> AddComponentVersion(
            AddComponentVersionInput input,
            [DataLoader] ComponentVersionDataLoader componentVersionLoader
            )
        {
            return AddAssociation<
              AddComponentVersionInput,
              ValueObjects.AddComponentVersionInput,
              ComponentVersion,
              AddComponentVersionPayload
              >(
                input,
                AddComponentVersionInput.Validate,
                association => new AddComponentVersionPayload(association),
                componentVersionLoader
              );
        }

        public Task<RemoveComponentVersionPayload> RemoveComponentVersion(
            RemoveComponentVersionInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.ComponentVersion,
              RemoveComponentVersionInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>,
              RemoveComponentVersionPayload
              >(
                input,
                RemoveComponentVersionInput.Validate,
                (baseComponentId, versionComponentId, requestTimestamp) => new RemoveComponentVersionPayload(baseComponentId, versionComponentId, requestTimestamp)
              );
        }

        public Task<AddInstitutionRepresentativePayload> AddInstitutionRepresentative(
            AddInstitutionRepresentativeInput input,
            [DataLoader] InstitutionRepresentativeDataLoader institutionRepresentativeLoader
            )
        {
            return AddAssociation<
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

        public Task<RemoveInstitutionRepresentativePayload> RemoveInstitutionRepresentative(
            RemoveInstitutionRepresentativeInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.InstitutionRepresentative,
              RemoveInstitutionRepresentativeInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>,
              RemoveInstitutionRepresentativePayload
              >(
                input,
                RemoveInstitutionRepresentativeInput.Validate,
                (institutionId, userId, requestTimestamp) => new RemoveInstitutionRepresentativePayload(institutionId, userId, requestTimestamp)
              );
        }

        public Task<AddMethodDeveloperPayload> AddMethodDeveloper(
            AddMethodDeveloperInput input,
            [DataLoader] MethodDeveloperDataLoader methodDeveloperLoader
            )
        {
            return AddAssociation<
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

        public Task<RemoveMethodDeveloperPayload> RemoveMethodDeveloper(
            RemoveMethodDeveloperInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.MethodDeveloper,
              RemoveMethodDeveloperInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>,
              RemoveMethodDeveloperPayload
              >(
                input,
                RemoveMethodDeveloperInput.Validate,
                (methodId, stakeholderId, requestTimestamp) => new RemoveMethodDeveloperPayload(methodId, stakeholderId, requestTimestamp)
              );
        }

        public Task<AddPersonAffiliationPayload> AddPersonAffiliation(
            AddPersonAffiliationInput input,
            [DataLoader] PersonAffiliationDataLoader personAffiliationLoader
            )
        {
            return AddAssociation<
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

        public Task<RemovePersonAffiliationPayload> RemovePersonAffiliation(
            RemovePersonAffiliationInput input
            )
        {
            return RemoveManyToManyAssociation<
              Models.PersonAffiliation,
              RemovePersonAffiliationInput,
              ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>,
              RemovePersonAffiliationPayload
              >(
                input,
                RemovePersonAffiliationInput.Validate,
                (personId, institutionId, requestTimestamp) => new RemovePersonAffiliationPayload(personId, institutionId, requestTimestamp)
              );
        }
    }
}