import { isTruthy } from "../../../lib/array";
import { GeometricDataPartialFragment } from "../../../queries/data.generated";
import Float, { Unit } from "../../Float";
import InlineList from "../../InlineList";
import DataSummary from "../DataSummary";

export default function GeometricDataSummary({
  entity,
}: {
  entity: GeometricDataPartialFragment;
}) {
  const values = [
    entity.thicknesses.length > 0 && (
      <div key="thicknesses">
        Thicknesses{" "}
        <InlineList
          items={entity.thicknesses}
          renderItem={(item, index) => (
            <Float key={index} value={item} unit={Unit.METER} />
          )}
        />
      </div>
    ),
  ].filter(isTruthy);

  return (
    <DataSummary entity={entity}>{values.length > 0 && values}</DataSummary>
  );
}
