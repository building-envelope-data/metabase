import { HygrothermalDataPartialFragment } from "../../../queries/data.generated";
import paths from "../../../paths";
import DataSummary from "../DataSummary";

export default function HygrothermalDataSummary({
  entity,
}: {
  entity: HygrothermalDataPartialFragment;
}) {
  return (
    <DataSummary entity={entity} route={paths.hygrothermalData}></DataSummary>
  );
}
