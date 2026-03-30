import { OpticalDataPartialFragment } from "../../../queries/data.generated";
import Float from "../../Float";
import InlineList from "../../InlineList";
import DataSummary from "../DataSummary";
import CielabColorViewer from "../../CielabColorViewer";
import { CoatedSide } from "../../../__generated__/graphql";
import { isTruthy } from "../../../lib/array";

export default function OpticalDataSummary({
  entity,
}: {
  entity: OpticalDataPartialFragment;
}) {
  const values = [
    entity.coatedSide && entity.coatedSide != CoatedSide.NotApplicable && (
      <div key="coatedSide">Coated Side {entity.coatedSide}</div>
    ),
    entity.infraredEmittances.length > 0 && (
      <div key="infraredEmittances">
        Infrared Emittances{" "}
        <InlineList
          items={entity.infraredEmittances}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
    entity.nearnormalHemisphericalSolarReflectances.length > 0 && (
      <div key="nearnormalHemisphericalSolarReflectances">
        Near-Normal Hemispherical Solar Reflectances{" "}
        <InlineList
          items={entity.nearnormalHemisphericalSolarReflectances}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
    entity.nearnormalHemisphericalSolarTransmittances.length > 0 && (
      <div key="nearnormalHemisphericalSolarTransmittances">
        Near-Normal Hemispherical Solar Transmittances{" "}
        <InlineList
          items={entity.nearnormalHemisphericalSolarTransmittances}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
    entity.nearnormalHemisphericalVisibleReflectances.length > 0 && (
      <div key="nearnormalHemisphericalVisibleReflectances">
        Near-Normal Hemispherical Visible Reflectances{" "}
        <InlineList
          items={entity.nearnormalHemisphericalVisibleReflectances}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
    entity.nearnormalHemisphericalVisibleTransmittances.length > 0 && (
      <div key="nearnormalHemisphericalVisibleTransmittances">
        Near-Normal Hemispherical Visible Transmittances{" "}
        <InlineList
          items={entity.nearnormalHemisphericalVisibleTransmittances}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
    entity.cielabColors.length > 0 && (
      <div key="cielabColors">
        CIELAB Color Space{" "}
        <InlineList
          items={entity.cielabColors}
          renderItem={(item, index) => (
            <CielabColorViewer key={index} value={item} />
          )}
        />
      </div>
    ),
    entity.colorRenderingIndices.length > 0 && (
      <div key="colorRenderingIndices">
        Color Rendering Indices{" "}
        <InlineList
          items={entity.colorRenderingIndices}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
  ].filter(isTruthy);

  return (
    <DataSummary entity={entity}>{values.length > 0 && values}</DataSummary>
  );
}
