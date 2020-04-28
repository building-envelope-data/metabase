using NSwag.Generation.Processors.Security;
using IdentityServer4.AccessTokenValidation;
using Icon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Microsoft.OpenApi;
using NSwag;
using IdentityServer4.AspNetIdentity;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Command = Icon.Infrastructure.Command;
using Query = Icon.Infrastructure.Query;
using Events = Icon.Events;
using Models = Icon.Models;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Queries = Icon.Queries;
using Commands = Icon.Commands;
using Handlers = Icon.Handlers;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using MediatR;

namespace Icon.Configuration
{
    public sealed class QueryCommandAndEventBusses
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));

            /* services.AddScoped<MediatR.IMediator, MediatR.Mediator>(); */
            /* services.AddTransient<MediatR.ServiceFactory>(sp => t => sp.GetService(t)); */

            services.AddScoped<Query.IQueryBus, Query.QueryBus>();
            services.AddScoped<Command.ICommandBus, Command.CommandBus>();
            services.AddScoped<Events.IEventBus, Events.EventBus>();

            // GetModelsAtTimestamps
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Component>,
              IEnumerable<Result<IEnumerable<Result<Models.Component, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Component, Aggregates.ComponentAggregate, Events.ComponentCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Database>,
              IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Database, Aggregates.DatabaseAggregate, Events.DatabaseCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Institution, Aggregates.InstitutionAggregate, Events.InstitutionCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Method>,
              IEnumerable<Result<IEnumerable<Result<Models.Method, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Method, Aggregates.MethodAggregate, Events.MethodCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Person>,
              IEnumerable<Result<IEnumerable<Result<Models.Person, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Person, Aggregates.PersonAggregate, Events.PersonCreated>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<Models.Standard>,
              IEnumerable<Result<IEnumerable<Result<Models.Standard, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<Models.Standard, Aggregates.StandardAggregate, Events.StandardCreated>
                >();

            // GetModelsForTimestampedIdsHandler
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.IModel>,
              IEnumerable<Result<Models.IModel, Errors>>
                >,
              Handlers.GetModelsOfUnknownTypeForTimestampedIdsHandler
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Component>,
              IEnumerable<Result<Models.Component, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Component, Aggregates.ComponentAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Database>,
              IEnumerable<Result<Models.Database, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Database, Aggregates.DatabaseAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Institution>,
              IEnumerable<Result<Models.Institution, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Method>,
              IEnumerable<Result<Models.Method, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Method, Aggregates.MethodAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Person>,
              IEnumerable<Result<Models.Person, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Stakeholder>,
              IEnumerable<Result<Models.Stakeholder, Errors>>
                >,
              Handlers.GetStakeholdersForTimestampedIdsHandler
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Standard>,
              IEnumerable<Result<Models.Standard, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.Standard, Aggregates.StandardAggregate>
                >();

            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.ComponentManufacturer>,
              IEnumerable<Result<Models.ComponentManufacturer, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.InstitutionRepresentative>,
              IEnumerable<Result<Models.InstitutionRepresentative, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.MethodDeveloper>,
              IEnumerable<Result<Models.MethodDeveloper, Errors>>
                >,
              Handlers.GetMethodDevelopersForTimestampedIdsHandler
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.PersonAffiliation>,
              IEnumerable<Result<Models.PersonAffiliation, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<Models.PersonAffiliation, Aggregates.PersonAffiliationAggregate>
                >();

            // GetAssociatesOfModelsIdentifiedByTimestampedIds
            /* services.AddScoped< */
            /*   MediatR.IRequestHandler< */
            /*   Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Component, Models.Institution>, */
            /*   IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>> */
            /*     >, */
            /*   Handlers.GetComponentManufacturersForTimestampedComponentIdsHandler */
            /*     >(); */
            /* services.AddScoped< */
            /*   MediatR.IRequestHandler< */
            /*   Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.X, Models.Y>, */
            /*   IEnumerable<Result<IEnumerable<Result<Models.Y, Errors>>, Errors>> */
            /*     >, */
            /*   Handlers.GetXYsForTimestampedXIdsHandler */
            /*     >(); */

            AddComponentManufacturerAssociationHandlers(services);
            AddInstitutionRepresentativeAssociationHandlers(services);
            AddMethodDeveloperAssociationHandlers(services);
            AddPersonAffiliationAssociationHandlers(services);

            /* services.AddScoped< */
            /*   MediatR.IRequestHandler< */
            /*   Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.Stakeholder>, */
            /*   IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>> */
            /*     >, */
            /*   Handlers.GetRepresentativesOfInstitutionsIdentifiedByTimestampedIdsHandler */
            /*     >(); */

            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateComponent,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateComponentHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateDatabase,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateDatabaseHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateInstitution,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateInstitutionHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateMethod,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateMethodHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreatePerson,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreatePersonHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateStandard,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateStandardHandler>();

            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddComponentManufacturer,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddComponentManufacturerHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddInstitutionRepresentative,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddInstitutionRepresentativeHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddMethodDeveloper,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddMethodDeveloperHandler>();
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddPersonAffiliation,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddPersonAffiliationHandler>();

            // TODO Shall we broadcast events?
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }

        public static void AddComponentManufacturerAssociationHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Component, Models.ComponentManufacturer>,
              IEnumerable<Result<IEnumerable<Result<Models.ComponentManufacturer, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.Component, Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.ComponentManufacturer>,
              IEnumerable<Result<IEnumerable<Result<Models.ComponentManufacturer, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>
                >();

            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Component, Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Component, Models.Institution, Aggregates.InstitutionAggregate, Events.ComponentManufacturerAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.Component>,
              IEnumerable<Result<IEnumerable<Result<Models.Component, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Component, Aggregates.ComponentAggregate, Events.ComponentManufacturerAdded>
                >();
        }

        public static void AddInstitutionRepresentativeAssociationHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.InstitutionRepresentative>,
              IEnumerable<Result<IEnumerable<Result<Models.InstitutionRepresentative, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate, Events.InstitutionRepresentativeAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.User, Models.InstitutionRepresentative>,
              IEnumerable<Result<IEnumerable<Result<Models.InstitutionRepresentative, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociationsOfModelsIdentifiedByTimestampedIdsHandler<Models.User, Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate, Events.InstitutionRepresentativeAdded>
                >();

            // TODO Add the following.
            /* services.AddScoped< */
            /*   MediatR.IRequestHandler< */
            /*   Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.User>, */
            /*   IEnumerable<Result<IEnumerable<Result<Models.User, Errors>>, Errors>> */
            /*     >, */
            /*   Handlers.GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.User, Aggregates.UserAggregate, Events.InstitutionRepresentativeAdded> */
            /*     >(); */
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.User, Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.User, Models.Institution, Aggregates.InstitutionAggregate, Events.InstitutionRepresentativeAdded>
                >();
        }

        public static void AddMethodDeveloperAssociationHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.Stakeholder>,
              IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>
                >,
              Handlers.GetDevelopersOfMethodsIdentifiedByTimestampedIdsHandler
                >();

            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Person, Models.Method>,
              IEnumerable<Result<IEnumerable<Result<Models.Method, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Person, Models.Method, Aggregates.MethodAggregate, Events.PersonMethodDeveloperAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.Method>,
              IEnumerable<Result<IEnumerable<Result<Models.Method, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Method, Aggregates.MethodAggregate, Events.InstitutionMethodDeveloperAdded>
                >();
        }

        public static void AddPersonAffiliationAssociationHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Person, Models.Institution>,
              IEnumerable<Result<IEnumerable<Result<Models.Institution, Errors>>, Errors>>
                >,
              Handlers.GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Person, Models.Institution, Aggregates.InstitutionAggregate, Events.PersonAffiliationAdded>
                >();
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Institution, Models.Person>,
              IEnumerable<Result<IEnumerable<Result<Models.Person, Errors>>, Errors>>
                >,
              Handlers.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Institution, Models.Person, Aggregates.PersonAggregate, Events.PersonAffiliationAdded>
                >();
        }
    }
}