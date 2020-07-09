using Infrastructure.Aggregates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Database.Configuration
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
            // `ls -1 src/Aggregates/ | grep -E ".*Aggregate.cs$" | grep -v -E "^DataAggregate.cs$" | sed -e "s/^\(.*\).cs\$/options.Events.InlineProjections.AggregateStreamsWith<Aggregates.\1>();/"`
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.CalorimetricDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.HygrothermalDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.OpticalDataAggregate>();
            options.Events.InlineProjections.AggregateStreamsWith<Aggregates.PhotovoltaicDataAggregate>();
        }

        private static void AddEventTypes(Marten.StoreOptions options)
        {
            // Originally generated from the command line with the command
            // `ls -1 src/Events/ | grep "Created\|Added\|Deleted\|Removed" | grep -v -E "^Data(Created|Deleted)Event.cs$" | sed -e "s/^\(.*\).cs\$/options.Events.AddEventType(typeof(Events.\1));/"`
            options.Events.AddEventType(typeof(Events.CalorimetricDataCreated));
            options.Events.AddEventType(typeof(Events.CalorimetricDataDeleted));
            options.Events.AddEventType(typeof(Events.HygrothermalDataCreated));
            options.Events.AddEventType(typeof(Events.HygrothermalDataDeleted));
            options.Events.AddEventType(typeof(Events.OpticalDataCreated));
            options.Events.AddEventType(typeof(Events.OpticalDataDeleted));
            options.Events.AddEventType(typeof(Events.PhotovoltaicDataCreated));
            options.Events.AddEventType(typeof(Events.PhotovoltaicDataDeleted));
        }
    }
}