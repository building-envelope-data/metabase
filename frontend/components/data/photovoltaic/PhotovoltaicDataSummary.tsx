import { PhotovoltaicDataPartialFragment } from "../../../queries/data.generated";
import paths from "../../../paths";
import DataSummary from "../DataSummary";

export default function PhotovoltaicDataSummary({
  entity,
}: {
  entity: PhotovoltaicDataPartialFragment;
}) {
  return (
    <DataSummary entity={entity} route={paths.photovoltaicData}></DataSummary>
  );
}
