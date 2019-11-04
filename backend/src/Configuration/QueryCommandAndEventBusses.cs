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
using Event = Icon.Infrastructure.Event;
using Models = Icon.Models;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Queries = Icon.Queries;
using Commands = Icon.Commands;
using Handlers = Icon.Handlers;

namespace Icon.Configuration
{
    class QueryCommandAndEventBusses
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MediatR.IMediator, MediatR.Mediator>();
            services.AddTransient<MediatR.ServiceFactory>(sp => t => sp.GetService(t));

            services.AddScoped<Query.IQueryBus, Query.QueryBus>();
            services.AddScoped<Command.ICommandBus, Command.CommandBus>();
            services.AddScoped<Event.IEventBus, Event.EventBus>();

            services.AddScoped<MediatR.IRequestHandler<Queries.ListComponents, IEnumerable<Models.Component>>, Handlers.ListComponentsHandler>();
            services.AddScoped<MediatR.IRequestHandler<Queries.GetComponent, Models.Component>, Handlers.GetComponentHandler>();
            services.AddScoped<MediatR.IRequestHandler<Queries.ListComponentVersions, IEnumerable<Models.ComponentVersion>>, Handlers.ListComponentVersionsHandler>();
            /* services.AddScoped<MediatR.IRequestHandler<Queries.GetComponentVersion, Models.ComponentVersion>, Handlers.GetComponentVersionHandler>(); */

            services.AddScoped<MediatR.IRequestHandler<Commands.CreateComponent, Models.Component>, Handlers.CreateComponentHandler>();
            services.AddScoped<MediatR.IRequestHandler<Commands.CreateComponentVersion, Models.ComponentVersion>, Handlers.CreateComponentVersionHandler>();
            services.AddScoped<MediatR.IRequestHandler<Commands.CreateComponentVersionManufacturer, Models.ComponentVersionManufacturer>, Handlers.CreateComponentVersionManufacturerHandler>();

            // TODO Shall we broadcast events?
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }
    }
}