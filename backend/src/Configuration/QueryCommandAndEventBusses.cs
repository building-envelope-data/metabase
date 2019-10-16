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
using Domain = Icon.Domain;
using Component = Icon.Domain.Component;
using ComponentVersion = Icon.Domain.ComponentVersion;
using ComponentVersionOwnership = Icon.Domain.ComponentVersionOwnership;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;

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

            services.AddScoped<MediatR.IRequestHandler<Component.List.Query, IEnumerable<Domain.ComponentAggregate>>, Component.List.QueryHandler>();
            services.AddScoped<MediatR.IRequestHandler<Component.Get.Query, Domain.ComponentView>, Component.Get.QueryHandler>();
            services.AddScoped<MediatR.IRequestHandler<Component.Create.Command, Domain.ComponentAggregate>, Component.Create.CommandHandler>();

            services.AddScoped<MediatR.IRequestHandler<ComponentVersion.Create.Command, Domain.ComponentVersionAggregate>, ComponentVersion.Create.CommandHandler>();
            services.AddScoped<MediatR.IRequestHandler<ComponentVersionOwnership.Create.Command, Domain.ComponentVersionOwnershipAggregate>, ComponentVersionOwnership.Create.CommandHandler>();
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }
    }
}