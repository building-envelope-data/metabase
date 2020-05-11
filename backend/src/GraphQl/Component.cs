using Models = Icon.Models;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.GraphQl
{
    public sealed class Component
      : NodeBase
    {
        public static Component FromModel(
            Models.Component model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new Component(
                id: model.Id,
                information: ComponentInformation.FromModel(model.Information),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ComponentInformation Information { get; }

        public Component(
            ValueObjects.Id id,
            ComponentInformation information,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            Information = information;
        }

        public ComponentManufacturerConnection GetManufacturers(
            [Parent] Component component
            )
        {
            return new ComponentManufacturerConnection(component);
        }

        public Task<IReadOnlyList<Component>> GetConcretizations(
            [Parent] Component component,
            [DataLoader] ConcretizationsOfComponentIdentifiedByTimestampedIdDataLoader concretizationsLoader
            )
        {
            return concretizationsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class ConcretizationsOfComponentIdentifiedByTimestampedIdDataLoader
            : ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentConcretization, Models.Component>
        {
            public ConcretizationsOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetGeneralizations(
            [Parent] Component component,
            [DataLoader] GeneralizationsOfComponentIdentifiedByTimestampedIdDataLoader generalizationsLoader
            )
        {
            return generalizationsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class GeneralizationsOfComponentIdentifiedByTimestampedIdDataLoader
            : BackwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentConcretization, Models.Component>
        {
            public GeneralizationsOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetParts(
            [Parent] Component component,
            [DataLoader] PartsOfComponentIdentifiedByTimestampedIdDataLoader partsLoader
            )
        {
            return partsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class PartsOfComponentIdentifiedByTimestampedIdDataLoader
            : ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentPart, Models.Component>
        {
            public PartsOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetPartOf(
            [Parent] Component component,
            [DataLoader] PartOfOfComponentIdentifiedByTimestampedIdDataLoader partOfLoader
            )
        {
            return partOfLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class PartOfOfComponentIdentifiedByTimestampedIdDataLoader
            : BackwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentPart, Models.Component>
        {
            public PartOfOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetVariants(
            [Parent] Component component,
            [DataLoader] VariantsOfComponentIdentifiedByTimestampedIdDataLoader variantsLoader
            )
        {
            return variantsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VariantsOfComponentIdentifiedByTimestampedIdDataLoader
            : ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentVariant, Models.Component>
        {
            public VariantsOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        // TODO If we enforce `ComponentVariant` to be symmetric, then `variantOf` is obsolete
        public Task<IReadOnlyList<Component>> GetVariantOf(
            [Parent] Component component,
            [DataLoader] VariantOfOfComponentIdentifiedByTimestampedIdDataLoader variantOfLoader
            )
        {
            return variantOfLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VariantOfOfComponentIdentifiedByTimestampedIdDataLoader
            : BackwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentVariant, Models.Component>
        {
            public VariantOfOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetVersions(
            [Parent] Component component,
            [DataLoader] VersionsOfComponentIdentifiedByTimestampedIdDataLoader versionsLoader
            )
        {
            return versionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VersionsOfComponentIdentifiedByTimestampedIdDataLoader
            : ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentVersion, Models.Component>
        {
            public VersionsOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetVersionOf(
            [Parent] Component component,
            [DataLoader] VersionOfOfComponentIdentifiedByTimestampedIdDataLoader versionOfLoader
            )
        {
            return versionOfLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VersionOfOfComponentIdentifiedByTimestampedIdDataLoader
            : BackwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Component, Models.Component, Models.ComponentVersion, Models.Component>
        {
            public VersionOfOfComponentIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}