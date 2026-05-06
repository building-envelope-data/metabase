import { isTruthy } from "../../../lib/array";
import { CalorimetricDataPartialFragment } from "../../../queries/data.generated";
import Float from "../../Float";
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
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
    entity.uValues.length > 0 && (
      <div key="uValues">
        U-Values{" "}
        <InlineList
          items={entity.uValues}
          renderItem={(item, index) => <Float key={index} value={item} />}
        />
      </div>
    ),
  ].filter(isTruthy);

  return (
    <DataSummary entity={entity}>{values.length > 0 && values}</DataSummary>
  );
}
