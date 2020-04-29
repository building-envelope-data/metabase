/* using Models = Icon.Models; */
/* using GreenDonut; */
/* using DateTime = System.DateTime; */
/* using CancellationToken = System.Threading.CancellationToken; */
/* using HotChocolate; */
/* using IQueryBus = Icon.Infrastructure.Query.IQueryBus; */
/* using IResolverContext = HotChocolate.Resolvers.IResolverContext; */
/* using System.Collections.Generic; */
/* using System.Threading.Tasks; */
/* using IPageInfo = HotChocolate.Types.Relay.IPageInfo; */

/* namespace Icon.GraphQl */
/* { */
/*     public sealed class ManufacturedComponentConnection */
/*     { */
/*         public IPageInfo PageInfo { get; } */

/*         public ManufacturedComponentConnection( */
/*             string name, */
/*             ContactInformation contactInformation, */
/*             ValueObjects.Timestamp timestamp, */
/*             DateTime requestTimestamp */
/*             ) */
/*           : base( */
/*               id: id, */
/*               timestamp: timestamp, */
/*               requestTimestamp: requestTimestamp */
/*               ) */
/*         { */
/*             Name = name; */
/*             ContactInformation = contactInformation; */
/*         } */

/*         public Task<IReadOnlyList<Component>> GetManufacturedComponents( */
/*             [Parent] Institution institution, */
/*             [DataLoader] ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdDataLoader manufacturedComponentsLoader, */
/*             IResolverContext context */
/*             ) */
/*         { */
/*             return manufacturedComponentsLoader.LoadAsync( */
/*                 TimestampHelpers.TimestampId(institution.Id, TimestampHelpers.Fetch(context)) */
/*                 ); */
/*         } */

/*         public sealed class ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdDataLoader */
/*             : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Institution, Models.Component> */
/*         { */
/*             public ComponentsManufacturedByInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus) */
/*               : base(Component.FromModel, queryBus) */
/*             { */
/*             } */
/*         } */

/*     } */
/* } */