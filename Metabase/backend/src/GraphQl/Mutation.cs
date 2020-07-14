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

namespace Metabase.GraphQl
{
    public sealed class Mutation
      : Infrastructure.GraphQl.Mutation
    {
        private readonly UserManager<Models.UserX> _userManager;

        public Mutation(ICommandBus commandBus, IQueryBus queryBus, UserManager<Models.UserX> userManager)
          : base(commandBus, queryBus)
        {
            _userManager = userManager;
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
              RemovePersonAffiliationPayload
              >(
                input,
                RemovePersonAffiliationInput.Validate,
                (personId, institutionId, requestTimestamp) => new RemovePersonAffiliationPayload(personId, institutionId, requestTimestamp)
              );
        }
    }
}