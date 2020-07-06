using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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

        // TODO Get*Data and WhoHas*Data (as in Query)
        /* public Task<IReadOnlyList<OpticalData>> GetOpticalData( */
        /*     [Parent] Component component, */
        /*     [DataLoader] OpticalDataOfComponentDataLoader opticalDataLoader */
        /*     ) */
        /* { */
        /*     return opticalDataLoader.LoadAsync( */
        /*         TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp) */
        /*         ); */
        /* } */

        /* public async Task<IReadOnlyList<object>> GetOpticalData( */
        /*     [Parent] Component component, */
        /*     [DataLoader] OpticalDataOfComponentDataLoader opticalDataLoader */
        /*     ) */
        /* { */
        /*   return (await */
        /*       opticalDataLoader.LoadAsync( */
        /*         TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp) */
        /*         ) */
        /*       .ConfigureAwait(false) */
        /*       ) */
        /*     .Select(opticalData => opticalData.Data) */
        /*     .ToList().AsReadOnly(); */
        /* } */

        /* public IReadOnlyList<Database> GetWhoHasOpticalData( */
        /*     [Parent] Component component */
        /*     ) */
        /* { */
        /*     return new Database[] { }; */
        /* } */

        public ComponentManufacturerConnection GetManufacturers(
            [Parent] Component component
            )
        {
            return new ComponentManufacturerConnection(component);
        }

        public Task<IReadOnlyList<Component>> GetConcretizations(
            [Parent] Component component,
            [DataLoader] ConcretizationsOfComponentDataLoader concretizationsLoader
            )
        {
            return concretizationsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class ConcretizationsOfComponentDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentConcretization, Models.Component>
        {
            public ConcretizationsOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetGeneralizations(
            [Parent] Component component,
            [DataLoader] GeneralizationsOfComponentDataLoader generalizationsLoader
            )
        {
            return generalizationsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class GeneralizationsOfComponentDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentConcretization, Models.Component>
        {
            public GeneralizationsOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetParts(
            [Parent] Component component,
            [DataLoader] PartsOfComponentDataLoader partsLoader
            )
        {
            return partsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class PartsOfComponentDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentPart, Models.Component>
        {
            public PartsOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetPartOf(
            [Parent] Component component,
            [DataLoader] PartOfOfComponentDataLoader partOfLoader
            )
        {
            return partOfLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class PartOfOfComponentDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentPart, Models.Component>
        {
            public PartOfOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetVariants(
            [Parent] Component component,
            [DataLoader] VariantsOfComponentDataLoader variantsLoader
            )
        {
            return variantsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VariantsOfComponentDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentVariant, Models.Component>
        {
            public VariantsOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        // TODO If we enforce `ComponentVariant` to be symmetric, then `variantOf` is obsolete
        public Task<IReadOnlyList<Component>> GetVariantOf(
            [Parent] Component component,
            [DataLoader] VariantOfOfComponentDataLoader variantOfLoader
            )
        {
            return variantOfLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VariantOfOfComponentDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentVariant, Models.Component>
        {
            public VariantOfOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetVersions(
            [Parent] Component component,
            [DataLoader] VersionsOfComponentDataLoader versionsLoader
            )
        {
            return versionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VersionsOfComponentDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentVersion, Models.Component>
        {
            public VersionsOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetVersionOf(
            [Parent] Component component,
            [DataLoader] VersionOfOfComponentDataLoader versionOfLoader
            )
        {
            return versionOfLoader.LoadAsync(
                TimestampHelpers.TimestampId(component.Id, component.RequestTimestamp)
                );
        }

        public sealed class VersionOfOfComponentDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Component, Models.Component, Models.ComponentVersion, Models.Component>
        {
            public VersionOfOfComponentDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}