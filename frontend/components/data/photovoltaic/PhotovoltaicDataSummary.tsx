import { PhotovoltaicDataPartialFragment } from "../../../queries/data.generated";
import DataSummary from "../DataSummary";

export default function PhotovoltaicDataSummary({
  entity,
}: {
  entity: PhotovoltaicDataPartialFragment;
}) {
  return <DataSummary entity={entity}></DataSummary>;
}
