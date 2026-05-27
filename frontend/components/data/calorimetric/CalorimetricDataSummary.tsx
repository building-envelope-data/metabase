import { isTruthy } from "../../../lib/array";
import paths from "../../../paths";
import { CalorimetricDataPartialFragment } from "../../../queries/data.generated";
import Float, { Unit } from "../../Float";
import InlineList from "../../InlineList";
import DataSummary from "../DataSummary";

export default function CalorimetricDataSummary({
  entity,
}: {
  entity: CalorimetricDataPartialFragment;
}) {
  const values = [
    entity.gValues.length > 0 && (
      <div key="gValues">
        G-Values{" "}
        <InlineList
          items={entity.gValues}
          renderItem={(item, index) => (
            <Float key={index} value={item} unit={Unit.UNITLESS} />
          )}
        />
      </div>
    ),
    entity.uValues.length > 0 && (
      <div key="uValues">
        U-Values{" "}
        <InlineList
          items={entity.uValues}
          renderItem={(item, index) => (
            <Float
              key={index}
              value={item}
              unit={Unit.WATT_PER_SQUARE_METER_KELVIN}
            />
          )}
        />
      </div>
    ),
  ].filter(isTruthy);

  return (
    <DataSummary entity={entity} route={paths.calorimetricData}>
      {values.length > 0 && values}
    </DataSummary>
  );
}
