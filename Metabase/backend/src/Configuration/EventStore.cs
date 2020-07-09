using Infrastructure.Aggregates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Metabase.Configuration
{
    public sealed class EventStore
      : Infrastructure.Configuration.EventStore
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            Infrastructure.AppSettings.DatabaseSettings databaseSettings
            )
        {
            Infrastructure.Configuration.EventStore.ConfigureServices(
                services,
                environment,
                databaseSettings,
                ProjectAggregatesInline,
                AddEventTypes
                );
        }

        private static void ProjectAggregatesInline(Marten.StoreOptions options)
        {
            // Originally generated from the command line with the command
            // `ls -1 src/Aggregates/ | grep -E ".*Aggregate.cs$" | grep -v -E "^MethodDeveloperAggregate.cs$" | grep -v -E "^I((Many|One)ToMany)?AssociationAggregate.cs$" | grep -v -E "^DataAggregate.cs$" | sed -e "s/^\(.*\).cs\$/options.Events.InlineProjections.AggregateStreamsWith<Aggregates.\1>();/"`
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentConcretizationAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentManufacturerAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentPartAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVariantAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVersionAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.DatabaseAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionMethodDeveloperAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionOperatedDatabaseAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.InstitutionRepresentativeAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.MethodAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PersonAffiliationAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PersonAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PersonMethodDeveloperAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.StandardAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.UserAggregate>();
        }

        private static void AddEventTypes(Marten.StoreOptions options)
        {
            // Originally generated from the command line with the command
            // `ls -1 src/Events/ | grep "Created\|Added\|Deleted\|Removed" | grep -v -E "^I?(Created|Deleted)Event.cs$" | grep -v -E "^Data(Created|Deleted)Event.cs$" | grep -v -E "^I?Association(Added|Removed)Event.cs$" | grep -v -E "^MethodDeveloperAdded.cs$" | sed -e "s/^\(.*\).cs\$/options.Events.AddEventType(typeof(Events.\1));/"`
            options.Events.AddEventType(typeof(Events.ComponentConcretizationAdded));
            options.Events.AddEventType(typeof(Events.ComponentConcretizationRemoved));
            options.Events.AddEventType(typeof(Events.ComponentCreated));
            options.Events.AddEventType(typeof(Events.ComponentDeleted));
            options.Events.AddEventType(typeof(Events.ComponentManufacturerAdded));
            options.Events.AddEventType(typeof(Events.ComponentManufacturerRemoved));
            options.Events.AddEventType(typeof(Events.ComponentPartAdded));
            options.Events.AddEventType(typeof(Events.ComponentPartRemoved));
            options.Events.AddEventType(typeof(Events.ComponentVariantAdded));
            options.Events.AddEventType(typeof(Events.ComponentVariantRemoved));
            options.Events.AddEventType(typeof(Events.ComponentVersionAdded));
            options.Events.AddEventType(typeof(Events.ComponentVersionRemoved));
            options.Events.AddEventType(typeof(Events.DatabaseCreated));
            options.Events.AddEventType(typeof(Events.DatabaseDeleted));
            options.Events.AddEventType(typeof(Events.InstitutionCreated));
            options.Events.AddEventType(typeof(Events.InstitutionDeleted));
            options.Events.AddEventType(typeof(Events.InstitutionMethodDeveloperAdded));
            options.Events.AddEventType(typeof(Events.InstitutionMethodDeveloperRemoved));
            options.Events.AddEventType(typeof(Events.InstitutionOperatedDatabaseAdded));
            options.Events.AddEventType(typeof(Events.InstitutionOperatedDatabaseRemoved));
            options.Events.AddEventType(typeof(Events.InstitutionRepresentativeAdded));
            options.Events.AddEventType(typeof(Events.InstitutionRepresentativeRemoved));
            options.Events.AddEventType(typeof(Events.MethodCreated));
            options.Events.AddEventType(typeof(Events.MethodDeleted));
            options.Events.AddEventType(typeof(Events.MethodDeveloperRemoved));
            options.Events.AddEventType(typeof(Events.PersonAffiliationAdded));
            options.Events.AddEventType(typeof(Events.PersonAffiliationRemoved));
            options.Events.AddEventType(typeof(Events.PersonCreated));
            options.Events.AddEventType(typeof(Events.PersonDeleted));
            options.Events.AddEventType(typeof(Events.PersonMethodDeveloperAdded));
            options.Events.AddEventType(typeof(Events.PersonMethodDeveloperRemoved));
            options.Events.AddEventType(typeof(Events.StandardCreated));
            options.Events.AddEventType(typeof(Events.StandardDeleted));
            options.Events.AddEventType(typeof(Events.UserCreated));
            options.Events.AddEventType(typeof(Events.UserDeleted));
        }
    }
}