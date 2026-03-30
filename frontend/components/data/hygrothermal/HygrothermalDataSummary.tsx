import { HygrothermalDataPartialFragment } from "../../../queries/data.generated";
import DataSummary from "../DataSummary";

export default function HygrothermalDataSummary({
  entity,
}: {
  entity: HygrothermalDataPartialFragment;
}) {
  return <DataSummary entity={entity}></DataSummary>;
}
